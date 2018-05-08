#define DEBUG_CONTROLLERBASE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

using ECN_Framework_Entities.Accounts;

using EmailMarketing.API.Attributes;

using System.Diagnostics;
using System.Text;
using System.Web.Http.Controllers;


namespace EmailMarketing.API.Controllers
{
    [RaisesInvalidMessageOnModelError]
    /// <summary>
    /// Provides an abstraction from AuthenticationControllerBase adding base implementation for search and bi-direction API/Framework model transformation
    /// </summary>
    /// <typeparam name="APIModel"></typeparam>
    /// <typeparam name="FrameworkModel"></typeparam>
    public abstract class SearchableApiControllerBase<APIModel, FrameworkModel> : AuthenticatedUserControllerBase
        where APIModel : new()
        where FrameworkModel : new()
        
    {
        #region abstract properties

        /// <summary>
        /// Lists the API Model properties exposed by the controller, generally a subset of those provided by the corresponding Framework model.
        /// </summary>
        abstract public string[] ExposedProperties { get; }

        /// <summary>
        /// When implemented in a derived class, gets the unique ID value for the given object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction] abstract public object GetID(APIModel model);

        /// <summary>
        /// When implemented in a derived class, gets the unique ID value for the given object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction] abstract public object GetID(FrameworkModel model);

        #endregion abstract properties

        // search implementation delegate

        internal delegate IEnumerable<FrameworkModel> SearchImplementation(
            SearchableApiControllerBase<APIModel, FrameworkModel> controller, 
            HttpControllerContext controllerContext, 
            List<Models.SearchProperty> searchQuery);

        #region Search support

        internal List<Models.SearchResult<APIModel>> SearchBaseMethod( Models.SearchBase query, SearchImplementation searchImplementation )
        {
            EnsureValidSearchQuery(query.SearchCriteria);

            return new List<Models.SearchResult<APIModel>>(
                  from x
                    in searchImplementation(this, ControllerContext, query.SearchCriteria)
                 where x != null
                select (new Models.SearchResult<APIModel>()
                       {
                           ApiObject = Transform(x, true),
                           Location = Url.Link(Strings.Routing.DefaultApiRouteName, new { id = GetID(x), controller = ControllerName })
                       }));
        }

        internal object GetConvertedQueryValue(List<Models.SearchProperty> query, string propertyName, Type valueType, int id = 0)
        {
            try
            {
                var prop = query.Where((x) => x.Name == propertyName).FirstOrDefault();
                if (null == prop || null == prop.ValueSet || string.IsNullOrEmpty(prop.ValueSet.ToString()))
                {
                    return null;
                }
                object convertedValue = Convert.ChangeType(prop.ValueSet, valueType);
                return convertedValue;
            }
            catch (FormatException systemformatException)
            {
                RaiseInvalidMessageException("Values passed for "+ propertyName + " is invalid.");
            }
            catch (ArgumentNullException argumentNullException)
            {
                // null query
            }
            catch(InvalidCastException castException)
            {
                // ignore
            }
            return null;

        }

        internal string GetQueryComparator(List<Models.SearchProperty> query, string propertyName)
        {
            if(query == null)
            {
                return null;
            }
            
            var prop = query.Where((x) => x.Name == propertyName).FirstOrDefault();
            if (null == prop)
            {
                return null;
            }

            return prop.Comparator;
        }

        /// <summary>
        /// Validates the search criteria for the request against the search configuration for the 
        /// controller identified via <see cref="ControllerName"/>
        /// </summary>
        /// <seealso cref="EmailMarketing.API.Search.SearchConfigurationGroup"/>
        /// <param name="query">search criteria</param>
        /// <exception cref="ECN_Framework_Common.Objects.ECNException">Exception raised in the event of an invalid query</exception>
        internal void EnsureValidSearchQuery(List<Models.SearchProperty> query)
        {
            EnsureValidSearchQuery(Search.SearchConfiguration.Library[ControllerName.ToLower()], query);
        }

        /// <summary>
        /// Validates the supplied search property list for the request against given search specifications
        /// </summary>
        /// <param name="searchConfiguration">specifics valid search criteria for a controller</param>
        /// <param name="query">specifications for a search</param>
        /// <exception cref="ECN_Framework_Common.Objects.ECNException">Exception raised in the event of an invalid query</exception>
        private static void EnsureValidSearchQuery(Search.SearchConfigurationGroup searchConfiguration, List<Models.SearchProperty> query)
        {
            List<ECN_Framework_Common.Objects.ECNError> errors = new List<ECN_Framework_Common.Objects.ECNError>();
            Action<string> addError = (s) =>
                errors.Add(new ECN_Framework_Common.Objects.ECNError()
                {
                    Entity = searchConfiguration.ExceptionEntity,
                    Method = searchConfiguration.ExceptionMethod,
                    ErrorMessage = s
                });
            System.Action raiseException = () =>
            {
                throw new ECN_Framework_Common.Objects.ECNException(errors, ECN_Framework_Common.Objects.Enums.ExceptionLayer.API);
            };

            if (false == searchConfiguration.AllowEmptySearch)
            {
                if (null == query || 1 > query.Where(x => x != null).Count())
                {
                    addError("search must be constrained; did you forget to supply a query?");
                    raiseException();
                }
            }            
            else if(query == null)
            {
                return;
            }

            // check for duplicate constraints on the same property
            // ZZZ: roll this into SearchConfiguration.cs as a validation method to enable us to allow things like:
            //      FieldName like '%foo%' and FieldName != 'foobar'
            foreach (var searchProperty in query.Where(x => x != null))
            {
                if (query.Where((x) => x.Name == searchProperty.Name).Count() > 1)
                {
                    addError(String.Format(@"duplicate constraint on property ""{0}""", searchProperty.Name));
                }
            }

            // check properties using configured validation
            foreach (var searchProperty in query.Where(x => x != null))
            {
                if (String.IsNullOrWhiteSpace(searchProperty.Name))
                {
                    addError(@"invalid search constraint: missing ""Name"".");
                }
                else if (false == searchConfiguration.ContainsKey(searchProperty.Name))
                {
                    addError(String.Format(@"invalid search constraint: no property with name ""{0}""", searchProperty.Name));
                }
                else if (String.IsNullOrWhiteSpace(searchProperty.Comparator))
            {
                    addError(@"invalid search constraint: missing ""Comparator"".");
                }
                else if (null == searchProperty.ValueSet)
                {
                    addError(@"invalid search constraint: missing ""ValueSet"".");
                }
                else
                {
                    var config = searchConfiguration[searchProperty.Name];
                    if (false == config.ValidationMethod(searchConfiguration, query, config, searchProperty))
                    {
                        addError(String.Format(@"invalid search on ""{0}""", searchProperty.Name));
                    }
                }
            }

            if (errors.Count > 0)
            {
                raiseException();
            }
        }

        #endregion Search support
        #region transformation support

        internal class TransformationEventArgs : EventArgs
        {
            internal enum TransformationDirection { ToAPIModel, ToFrameworkModel }
            #region constructors 
            public TransformationEventArgs()
            {

            }
            public TransformationEventArgs(APIModel apiModel)
            {
                Direction = TransformationDirection.ToFrameworkModel;
                ApiModel = apiModel;
            }
            public TransformationEventArgs(APIModel apiModel, FrameworkModel frameworkModel)
            {
                Direction = TransformationDirection.ToFrameworkModel;
                ApiModel = apiModel;
                FrameworkModel = frameworkModel;
            }
            public TransformationEventArgs(FrameworkModel frameworkModel)
            {
                Direction = TransformationDirection.ToAPIModel;
                FrameworkModel = frameworkModel;
            }
            public TransformationEventArgs(FrameworkModel frameworkModel, APIModel apiModel)
            {
                Direction = TransformationDirection.ToAPIModel;
                FrameworkModel = frameworkModel;
                ApiModel = apiModel;
            }
            
            #endregion constructors
            public TransformationDirection Direction;
            public bool IsCancelled = false;
            public APIModel ApiModel { get; set; }
            public FrameworkModel FrameworkModel { get; set; }
        }

        internal delegate void TransformationEventHandler(object sender, TransformationEventArgs args);
        
        internal event TransformationEventHandler OnBeforeTransformation;
        internal event TransformationEventHandler OnAfterTransformation;

        protected virtual FrameworkModel Transform(APIModel o)
        {
            return Transform(o, new FrameworkModel());
        }

        protected virtual FrameworkModel Transform(APIModel source, FrameworkModel destination)
        {
            TransformationEventArgs args = new TransformationEventArgs(source, destination);
            if (OnBeforeTransformation != null) OnBeforeTransformation(this, args);

            if (false == args.IsCancelled)
            {
                if (args.ApiModel == null) args.ApiModel = new APIModel();
                if (args.FrameworkModel == null) args.FrameworkModel = new FrameworkModel();
                args.FrameworkModel = Models.Utility.Transformer<APIModel, FrameworkModel>.Transform(args.ApiModel, args.FrameworkModel, ExposedProperties);                
                if (OnAfterTransformation != null) OnAfterTransformation(this, args);
            }

            return args.FrameworkModel;
        }

        protected virtual APIModel Transform(FrameworkModel o, bool useCustomAttributes = false)
        {
            return Transform(o, new APIModel(), useCustomAttributes);
        }

        

        protected virtual APIModel Transform(FrameworkModel source, APIModel destination, bool useCustomAttributes = false)
        {
            TransformationEventArgs args = new TransformationEventArgs(source, destination);
            if (OnBeforeTransformation != null) OnBeforeTransformation(this, args);

            if (false == args.IsCancelled)
            {
                if (args.ApiModel == null) args.ApiModel = new APIModel();
                if (args.FrameworkModel == null) args.FrameworkModel = new FrameworkModel();
                args.ApiModel = Models.Utility.Transformer<FrameworkModel, APIModel>.Transform(args.FrameworkModel, args.ApiModel, ExposedProperties, useCustomAttributes);                
                if (OnAfterTransformation != null) OnAfterTransformation(this, args);
            }

            return args.ApiModel;
        }

        #endregion transformation support
    }  

}

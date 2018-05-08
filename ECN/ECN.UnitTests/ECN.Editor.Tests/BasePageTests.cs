using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Editor.Tests
{
    [ExcludeFromCodeCoverage]
    public class BasePageTests : BaseControlTests
    {
        protected PrivateObject PrivatePage { get;set; }
        protected Page PortalPage { get; set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            PortalPage?.Dispose();
        }

        protected virtual void InitializePage(Page page)
        {
            PortalPage = page ?? throw new ArgumentNullException(nameof(page));
            PrivatePage = new PrivateObject(PortalPage);
            PrivateType = new PrivateType(page.GetType());
            InitializeAllControls(PortalPage);
        }

        protected override void InitializeAllControls(object page)
        {
            var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                try
                {
                    if (field.GetValue(page) == null)
                    {
                        var constructor = field.FieldType.GetConstructor(new Type[0]);
                        if (constructor != null)
                        {
                            var control = constructor.Invoke(new object[0]);
                            if (control != null)
                            {
                                field.SetValue(page, control);
                                TryLinkFieldWithPage(control, page);

                                if (control is UserControl)
                                {
                                    InitializeAllControls(control);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //Ignore exceptions related to UI controls initialization
                }
            }
        }

        private static void TryLinkFieldWithPage(object field, object page)
        {
            if (page is Page)
            {
                var fieldType = field.GetType().GetField("_page",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        protected override T GetField<T>(string fieldName)
        {
            var field = PrivatePage.GetField(fieldName) as T;

            field.ShouldNotBeNull($"The field {field} of type {typeof(T).Name} cannot be null");

            return field;
        }
    }
}

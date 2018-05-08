﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAS_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ITransformAssign
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.TransformAssign>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="TransformationID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.TransformAssign>> SelectForTransformation(Guid accessKey, int TransformationID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sourceFileID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.TransformAssign>> SelectForSourceFile(Guid accessKey, int sourceFileID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAS.Entity.TransformAssign x);
    }
}

// ﻿   (c) 2005-2017 Copyright, Real-Time Innovations, All rights reserved.
//
// RTI grants Licensee a license to use, modify, compile, and create
// derivative works of the Software.  Licensee has the right to distribute
// object form only for use with RTI products. The Software is provided
// "as is", with no warranty of any type, including any warranty for fitness
// for any purpose. RTI is under no obligation to maintain or support the
// Software.  RTI shall not be liable for any incidental or consequential
// damages arising out of the use or inability to use the software.
namespace RTI.Connext.Connector.Interface
{
    /// <summary>
    /// DDS function return code.
    /// </summary>
    enum ReturnCode
    {
        /// <summary>
        /// Successful return.
        /// </summary>
        Ok,

        /// <summary>
        /// Generic, unspecified error.
        /// </summary>
        Error,

        /// <summary>
        /// Unsupported operation.
        /// Can only returned by operations that are unsupported.
        /// </summary>
        Unsupported,

        /// <summary>
        /// Illegal parameter value.
        /// The value of the parameter that is passed in has llegal value.
        /// Things that fall into this category include NULL parameters and
        /// parameter values that are out of range. 
        /// </summary>
        BadParameter,

        /// <summary>
        /// A pre-condition for the operation was not met.
        /// The system is not in the expected state when the function is called,
        /// or the parameter itself is not in the expected state when the
        /// function is called. 
        /// </summary>
        PreconditionNotMet,

        /// <summary>
        /// RTI Connext ran out of the resources needed to complete the
        /// operation.
        /// </summary>
        OutOfResources,

        /// <summary>
        /// Operation invoked on a DDS_Entity that is not yet enabled.
        /// </summary>
        NotEnabled,

        /// <summary>
        /// Application attempted to modify an immutable QoS policy.
        /// </summary>
        InmutablePolicy,

        /// <summary>
        /// Application specified a set of QoS policies that are not consistent
        /// with each other. 
        /// </summary>
        InconsistencyPolicy,

        /// <summary>
        /// The object target of this operation has already been deleted.
        /// </summary>
        AlreadyDeleted,

        /// <summary>
        /// The operation timed out.
        /// </summary>
        Timeout,

        /// <summary>
        /// Indicates a transient situation where the operation did not return
        /// any data but there is no inherent error. 
        /// </summary>
        NoData,

        /// <summary>
        /// The operation was called under improper circumstances.
        /// An operation was invoked on an inappropriate object or at an
        /// inappropriate time. This return code is similar to
        /// <see cref="PreconditionNotMet"/>, except that there is no
        /// precondition that could be changed to make the operation succeed. 
        /// </summary>
        IllegalOperation,

        /// <summary>
        /// An operation on the DDS API that fails because the security plugins
        /// do not allow it.
        /// </summary>
        NotAllowedBySec
    }
}

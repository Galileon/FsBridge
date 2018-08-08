using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect
{
    public enum FsFunctionCallResult
    {
        Ok,
        InvalidSwitchId,
        InvalidCallId,
        Error,
        FileNotExists
    }


    public enum FsCallState
    {
        [EnumMember(Value = "DOWN")]
        Disconnected,
        [EnumMember(Value = "DIALING")]
        Dialing,
        [EnumMember(Value = "RINGING")]
        Ringing,
        [EnumMember(Value = "EARLY")]
        Early,
        [EnumMember(Value = "ACTIVE")]
        Active,
        [EnumMember(Value = "HELD")]
        Held,
        [EnumMember(Value = "RING_WAIT")]
        RingWait,
        [EnumMember(Value = "HANGUP")]
        Hangup,
        [EnumMember(Value = "UNHELD")]
        Unheld
    }

    public enum FsCallDirection
    {
        [EnumMember(Value = "INBOUND")]
        Inbound,
        [EnumMember(Value = "OUTBOUND")]
        Outbound
    }

    public enum FsPlaybackStatus
    {
        [EnumMember(Value = "done")]
        Done,
        [EnumMember(Value = "break")]
        Break,
        [EnumMember(Value = "File Not Found")]
        FileNotFound,
        [EnumMember(Value = "Error")]
        Error,
        [EnumMember(Value = "Disconnected")]
        Disconnected,
    }

    public enum FsBlindTrasnferStatus
    {
        Transferred,
        Failed
    }


    public enum FsTone
    {
        Busy,
        Unavaileable,
        Ringback,
    }


    public enum FsEventCause
    {
        NONE = 0,
        UNALLOCATED_NUMBER = 1,
        NO_ROUTE_TRANSIT_NET = 2,
        NO_ROUTE_DESTINATION = 3,
        CHANNEL_UNACCEPTABLE = 6,
        CALL_AWARDED_DELIVERED = 7,
        NORMAL_CLEARING = 16,
        USER_BUSY = 17,
        NO_USER_RESPONSE = 18,
        NO_ANSWER = 19,
        SUBSCRIBER_ABSENT = 20,
        CALL_REJECTED = 21,
        NUMBER_CHANGED = 22,
        REDIRECTION_TO_NEW_DESTINATION = 23,
        EXCHANGE_ROUTING_ERROR = 25,
        DESTINATION_OUT_OF_ORDER = 27,
        INVALID_NUMBER_FORMAT = 28,
        FACILITY_REJECTED = 29,
        RESPONSE_TO_STATUS_ENQUIRY = 30,
        NORMAL_UNSPECIFIED = 31,
        NORMAL_CIRCUIT_CONGESTION = 34,
        NETWORK_OUT_OF_ORDER = 38,
        NORMAL_TEMPORARY_FAILURE = 41,
        SWITCH_CONGESTION = 42,
        ACCESS_INFO_DISCARDED = 43,
        REQUESTED_CHAN_UNAVAIL = 44,
        PRE_EMPTED = 45,
        FACILITY_NOT_SUBSCRIBED = 50,
        OUTGOING_CALL_BARRED = 52,
        INCOMING_CALL_BARRED = 54,
        BEARERCAPABILITY_NOTAUTH = 57,
        BEARERCAPABILITY_NOTAVAIL = 58,
        SERVICE_UNAVAILABLE = 63,
        BEARERCAPABILITY_NOTIMPL = 65,
        CHAN_NOT_IMPLEMENTED = 66,
        FACILITY_NOT_IMPLEMENTED = 69,
        SERVICE_NOT_IMPLEMENTED = 79,
        INVALID_CALL_REFERENCE = 81,
        INCOMPATIBLE_DESTINATION = 88,
        INVALID_MSG_UNSPECIFIED = 95,
        MANDATORY_IE_MISSING = 96,
        MESSAGE_TYPE_NONEXIST = 97,
        WRONG_MESSAGE = 98,
        IE_NONEXIST = 99,
        INVALID_IE_CONTENTS = 100,
        WRONG_CALL_STATE = 101,
        RECOVERY_ON_TIMER_EXPIRE = 102,
        MANDATORY_IE_LENGTH_ERROR = 103,
        PROTOCOL_ERROR = 111,
        INTERWORKING = 127,
        SUCCESS = 142,
        ORIGINATOR_CANCEL = 487,
        CRASH = 500,
        SYSTEM_SHUTDOWN = 501,
        LOSE_RACE = 502,
        MANAGER_REQUEST = 503,
        BLIND_TRANSFER = 600,
        ATTENDED_TRANSFER = 601,
        ALLOTTED_TIMEOUT = 602,
        USER_CHALLENGE = 603,
        MEDIA_TIMEOUT = 604,
        PICKED_OFF = 605,
        USER_NOT_REGISTERED = 606,
        PROGRESS_TIMEOUT = 607,
        INVALID_GATEWAY = 608,
        GATEWAY_DOWN = 609,
        INVALID_URL = 610,
        INVALID_PROFILE = 611,
        NO_PICKUP = 612,
        SRTP_READ_ERROR = 613,
        // Somethimes when we cancel the call
        UNKNOWN = 614
    }


    // Record-Completion-Cause
    public enum FsRecordStatus
    {
        [EnumMember(Value = "success-silence")]
        Success,

        [EnumMember(Value = "uri-failure")]
        UriFailue,

        [EnumMember(Value = "empty-file")]
        EmptyFile,

        [EnumMember(Value = "no-input-timeout")]
        InitialSilence,

        [EnumMember(Value = "input-too-short")]
        ToShortFile,

        [EnumMember(Value = "success-maxtime")]
        MaxTime,

    }


    public enum SipResponseCode
    {
        #region Provinsional

        /// <summary>
        /// Extended search being performed may take a significant time so a forking proxy must send a 100 Trying response
        /// </summary>
        Trying = 100,

        /// <summary>
        /// Destination user agent received INVITE, and is alerting user of call
        /// </summary>
        Ringing = 180,

        /// <summary>
        /// Servers can optionally send this response to indicate a call is being forwarded
        /// </summary>
        CallIsBeingForwarded = 181,

        /// <summary>
        /// Indicates that the destination was temporarily unavailable, so the server has queued the call until the destination is available. A server may send multiple 182 responses to update progress of the queue
        /// </summary>
        Queued = 182,

        /// <summary>
        /// This response may be used to send extra information for a call which is still being set up
        /// </summary>
        SessionInProgress = 183,

        /// <summary>
        /// Can be used by User Agent Server to indicate to upstream SIP entities (including the User Agent Client (UAC)) that an early dialog has been terminated.
        /// </summary>
        EarlyDialogTerminated = 199,

        #endregion Provinsional

        #region Successful Responses

        /// <summary>
        /// Indicates the request was successful.
        /// </summary>
        OK = 200,

        /// <summary>
        /// Indicates that the request has been accepted for processing, but the processing has not been completed.(RFC 3265, RFC 6665)
        /// </summary>
        Accepted = 202,

        /// <summary>
        /// Indicates the request was successful, but the corresponding response will not be received
        /// </summary>
        NoNotification = 204,

        #endregion Successful Responses

        #region Redirection Responses

        /// <summary>
        /// The address resolved to one of several options for the user or client to choose between, which are listed in the message body or the message's Contact fields
        /// </summary>
        MultipleChoices = 300,

        /// <summary>
        /// The original Request-URI is no longer valid, the new address is given in the Contact header field, and the client should update any records of the original Request-URI with the new value.
        /// </summary>
        MovedPermanently = 301,

        /// <summary>
        /// The client should try at the address in the Contact field. If an Expires field is present, the client may cache the result for that period of time.[1]:§21.3.3
        /// </summary>
        MovedTemporarily = 302,

        /// <summary>
        /// The Contact field details a proxy that must be used to access the requested destination
        /// </summary>
        UseProxy = 305,

        /// <summary>
        /// The call failed, but alternatives are detailed in the message body.
        /// </summary>
        AlternativeService = 380,

        #endregion Redirection Responses

        #region Client Failure Responses

        /// <summary>
        /// The request could not be understood due to malformed syntax
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// The request requires user authentication. This response is issued by UASs and registrars
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        PaymentRequired = 402,

        /// <summary>
        /// The server understood the request, but is refusing to fulfill it
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// The server has definitive information that the user does not exist at the domain specified in the Request-URI. This status is also returned if the domain in the Request-URI does not match any of the domains handled by the recipient of the request
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// The method specified in the Request-Line is understood, but not allowed for the address identified by the Request-UR
        /// </summary>
        MethodNotAllowed = 405,

        /// <summary>
        /// The resource identified by the request is only capable of generating response entities that have content characteristics but not acceptable according to the Accept header field sent in the request
        /// </summary>
        NotAcceptable = 406,

        /// <summary>
        /// The request requires user authentication. This response is issued by proxys.
        /// </summary>
        ProxyAuthenticationRequired = 407,

        /// <summary>
        /// Couldn't find the user in time
        /// </summary>
        RequestTimeout = 408,

        /// <summary>
        /// User already registered (RFC 2543)
        /// </summary>
        Coflict = 409,

        /// <summary>
        /// The user existed once, but is not available here any more
        /// </summary>
        Gone = 410,

        /// <summary>
        /// Conditional Request Failed (RFC 3903)
        /// </summary>
        ConditionalRequestFailed = 412,

        /// <summary>
        /// Request body too large
        /// </summary>
        RequestEntityTooLarge = 413,

        /// <summary>
        /// The server is refusing to service the request because the Request-URI is longer than the server is willing to interpret
        /// </summary>
        RequestURITooLong = 414,

        /// <summary>
        /// Request body in a format not supported
        /// </summary>
        UnsupportedMediaType = 415,

        /// <summary>
        /// Request-URI is unknown to the server.
        /// </summary>
        UnsupportedURIScheme = 416,

        /// <summary>
        /// Unknown Resource-Priority
        /// </summary>
        UnknownResourcePriority = 417,

        /// <summary>
        /// Bad SIP Protocol Extension used, not understood by the server
        /// </summary>
        BadExtension = 420,

        /// <summary>
        /// he server needs a specific extension not listed in the Supported header.
        /// </summary>
        ExtensionRequired = 421,

        /// <summary>
        /// The received request contains a Session-Expires header field with a duration below the minimum timer
        /// </summary>
        SessionIntervalTooSmall = 422,

        /// <summary>
        /// Expiration time of the resource is too short
        /// </summary>
        IntervalTooBrief = 423,

        /// <summary>
        /// Bad Location Information (RFC 6442)
        /// </summary>
        BadLocationInformation = 424,

        /// <summary>
        /// Use Identity Header (RFC 4474)
        /// </summary>
        UseIdentityHeader = 428,

        /// <summary>
        /// Provide Referrer Identity (RFC 3892)
        /// </summary>
        ProvideReferrerIdentity = 429,

        /// <summary>
        /// Anonymity Disallowed (RFC 5079)
        /// </summary>
        AnonymityDisallowed = 430,

        /// <summary>
        /// Bad Identity-Info (RFC 4474)
        /// </summary>
        BadIdentityInfo = 436,

        /// <summary>
        /// supported Certificate (RFC 4474)
        /// </summary>
        UnsupportedCertificate = 437,

        /// <summary>
        /// Invalid Identity Header (RFC 4474)
        /// </summary>
        InvalidIdentityHeader = 438,

        /// <summary>
        /// A 470 (Consent Needed) response indicates that the request that triggered the response contained a URI list with at least one URI for which the relay had no permissions. A user agent server generating a 470 (Consent Needed) response SHOULD include a Permission-Missing header field in it. This header field carries the URI or URIs for which the relay had no permissions. (RFC5360)
        /// </summary>
        ConsentNeeded = 470,

        /// <summary>
        /// Callee currently unavailable
        /// </summary>
        TemporarilyUnavailable = 480,

        /// <summary>
        /// Server received a request that does not match any dialog or transaction
        /// </summary>
        TransactionDoesNotExist = 481,

        /// <summary>
        /// Server has detected a loop
        /// </summary>
        LoopDetected = 482,

        /// <summary>
        /// Max-Forwards header has reach value '0'.
        /// </summary>
        TooManyHops = 483,

        /// <summary>
        /// Request-URI incomplete
        /// </summary>
        AddressIncomplete = 484,

        /// <summary>
        /// Request-URI is ambiguous
        /// </summary>
        Ambiguous = 485,

        /// <summary>
        /// Busy
        /// </summary>
        BusyHere = 486,

        /// <summary>
        /// Request has terminated by bye or cancel
        /// </summary>
        RequestTerminated = 487,

        /// <summary>
        /// Some aspects of the session description of the Request-URI is not acceptable
        /// </summary>
        NotAcceptableHere = 488,

        /// <summary>
        /// Bad Event (RFC 3265, RFC 6665)
        /// </summary>
        BadEvent = 489,

        /// <summary>
        /// Server has some pending request from the same dialog
        /// </summary>
        RequestPending = 491,

        /// <summary>
        /// Request contains an encrypted MIME body, which recipient can not decrypt
        /// </summary>
        Undecipherable = 493,

        /// <summary>
        /// Security Agreement Required (RFC 3329)
        /// </summary>
        SecurityAgreementRequired = 494,

        #endregion Client Failure Responses

        #region Server Failure Responses

        /// <summary>
        /// The server could not fulfill the request due to some unexpected condition
        /// </summary>
        ServerInternalError = 500,

        /// <summary>
        /// The server does not have the ability to fulfill the request, such as because it does not recognize the request method. (Compare with 405 Method Not Allowed, where the server recognizes the method but does not allow or support it.
        /// </summary>
        NotImplemented = 501,

        /// <summary>
        /// The server is acting as a gateway or proxy, and received an invalid response from a downstream server while attempting to fulfill the request.
        /// </summary>
        BadGateway = 502,

        /// <summary>
        /// The server is undergoing maintenance or is temporarily overloaded and so cannot process the request. A "Retry-After" header field may specify when the client may reattempt its request.[
        /// </summary>
        ServiceUnavailable = 503,

        /// <summary>
        /// The server attempted to access another server in attempting to process the request, and did not receive a prompt response
        /// </summary>
        ServerTimeout = 504,

        /// <summary>
        /// The SIP protocol version in the request is not supported by the server
        /// </summary>
        VersionNotSupported = 505,

        /// <summary>
        /// The request message length is longer than the server can process.
        /// </summary>
        MessageTooLarge = 513,

        /// <summary>
        /// The server is unable or unwilling to meet some constraints specified in the offer
        /// </summary>
        PreconditionFailure = 580,

        #endregion Server Failure Responses

        #region Global Failure Responses

        /// <summary>
        /// All possible destinations are busy. Unlike the 486 response, this response indicates the destination knows there are no alternative destinations (such as a voicemail server) able to accept the call
        /// </summary>
        BusyEverywhere = 600,

        /// <summary>
        /// The destination does not wish to participate in the call, or cannot do so, and additionally the client knows there are no alternative destinations (such as a voicemail server) willing to accept the call
        /// </summary>
        Decline = 603,

        /// <summary>
        /// The server has authoritative information that the requested user does not exist anywhere
        /// </summary>
        DoesNotExistAnywhere = 604,

        /// <summary>
        /// The user's agent was contacted successfully but some aspects of the session description such as the requested media, bandwidth, or addressing style were not acceptable
        /// </summary>
        SessionNotAcceptable = 606,

        #endregion Global Failure Responses
    }

    public enum EventSocketConnectionType
    {
        Event,
        Command
    }

    public enum AudioDestination
    {
        Both,
        ALeg,
    }

    public static class Freeswitch
    {

        public static FsEventCause SipResponseCodeToFsCause (SipResponseCode code)
        {
            switch (code)
            {

                case SipResponseCode.OK:
                    return FsEventCause.NORMAL_CLEARING;

                case SipResponseCode.Forbidden:
                    return FsEventCause.INVALID_NUMBER_FORMAT;

                case SipResponseCode.NotFound:
                    return FsEventCause.UNALLOCATED_NUMBER;

                case SipResponseCode.ServiceUnavailable:
                case SipResponseCode.Unauthorized:
                    return FsEventCause.SERVICE_UNAVAILABLE;

                case SipResponseCode.SessionNotAcceptable:
                    return FsEventCause.NO_ROUTE_DESTINATION;

                case SipResponseCode.BusyEverywhere:
                case SipResponseCode.BusyHere:
                    return FsEventCause.USER_BUSY;
                default:
                    return FsEventCause.NORMAL_TEMPORARY_FAILURE;
            }

        }

    }



}

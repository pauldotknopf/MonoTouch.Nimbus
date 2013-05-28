using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace MonoTouch.Nimbus
{
	#region AFNetworking

	public enum AFHTTPClientParameterEncoding {
		AFFormURLParameterEncoding = 0,
		AFJSONParameterEncoding = 1,
		AFPropertyListParameterEncoding = 2
	}

	// @interface AFHTTPClient : NSObject <NSCoding, NSCopying>
	[BaseType (typeof (NSObject))]
	public partial interface AFHTTPClient {

        // The url used as the base for paths specified in methods such as `getPath:parameters:success:failure`
		// @property (readonly, nonatomic) NSURL *baseURL;
		[Export ("baseURL")]
		NSUrl BaseUrl { get; }

		// The string encoding used in constructing url requests. This is `NSUTF8StringEncoding` by default.
		// @property (nonatomic, assign) NSStringEncoding stringEncoding;
		[Export ("stringEncoding")]
		NSStringEncoding StringEncoding { get; set; }

		// The `AFHTTPClientParameterEncoding` value corresponding to how parameters are encoded into a request body. This is `AFFormURLParameterEncoding` by default.
 		// @warning Some nested parameter structures, such as a keyed array of hashes containing inconsistent keys (i.e. `@{@"": @[@{@"a" : @(1)}, @{@"b" : @(2)}]}`), cannot be unambiguously represented in query strings. It is strongly recommended that an unambiguous encoding, such as `AFJSONParameterEncoding`, is used when posting complicated or nondeterministic parameter structures.
		// @property (nonatomic, assign) AFHTTPClientParameterEncoding parameterEncoding;
		[Export ("parameterEncoding")]
		AFHTTPClientParameterEncoding ParameterEncoding { get; set; }

		// The operation queue which manages operations enqueued by the HTTP client.
		// @property (readonly, nonatomic) NSOperationQueue *operationQueue;
		[Export ("operationQueue")]
		NSOperationQueue OperationQueue { get; }

 		// Creates and initializes an `AFHTTPClient` object with the specified base URL.
 		// @param url The base URL for the HTTP client. This argument must not be `nil`.
 		// @return The newly-initialized HTTP client
		// + (AFHTTPClient *)clientWithBaseURL:(NSURL *)url;
		[Static, Export ("clientWithBaseURL:")]
		AFHTTPClient ClientWithBaseUrl (NSUrl url);

		// Initializes an `AFHTTPClient` object with the specified base URL.
 		// @param url The base URL for the HTTP client. This argument must not be `nil`.
 		// @discussion This is the designated initializer.
 		// @return The newly-initialized HTTP client
		// - (id)initWithBaseURL:(NSURL *)url;
		[Export ("initWithBaseURL:")]
		IntPtr Constructor (NSUrl url);

 		// Attempts to register a subclass of `AFHTTPRequestOperation`, adding it to a chain to automatically generate request operations from a URL request. 
 		// @param operationClass The subclass of `AFHTTPRequestOperation` to register
 		// @return `YES` if the registration is successful, `NO` otherwise. The only failure condition is if `operationClass` is not a subclass of `AFHTTPRequestOperation`.
 		// @discussion When `enqueueHTTPRequestOperationWithRequest:success:failure` is invoked, each registered class is consulted in turn to see if it can handle the specific request. The first class to return `YES` when sent a `canProcessRequest:` message is used to create an operation using `initWithURLRequest:` and do `setCompletionBlockWithSuccess:failure:`. There is no guarantee that all registered classes will be consulted. Classes are consulted in the reverse order of their registration. Attempting to register an already-registered class will move it to the top of the list.
		// - (BOOL)registerHTTPOperationClass:(Class)operationClass;
		[Export ("registerHTTPOperationClass:")]
		bool RegisterHTTPOperationClass (MonoTouch.ObjCRuntime.Class operationClass);

 		// Unregisters the specified subclass of `AFHTTPRequestOperation` from the chain of classes consulted when `-requestWithMethod:path:parameters` is called.
 		// @param operationClass The subclass of `AFHTTPRequestOperation` to register 
		// - (void)unregisterHTTPOperationClass:(Class)operationClass;
		[Export ("unregisterHTTPOperationClass:")]
		void UnregisterHTTPOperationClass (MonoTouch.ObjCRuntime.Class operationClass);

 		// Returns the value for the HTTP headers set in request objects created by the HTTP client.
 		// @param header The HTTP header to return the default value for
 		// @return The default value for the HTTP header, or `nil` if unspecified
		// - (NSString *)defaultValueForHeader:(NSString *)header;
		[Export ("defaultValueForHeader:")]
		string DefaultValueForHeader (string header);

 		// Sets the value for the HTTP headers set in request objects made by the HTTP client. If `nil`, removes the existing value for that header.
 		// @param header The HTTP header to set a default value for
 		// @param value The value set as default for the specified header, or `nil
		// - (void)setDefaultHeader:(NSString *)header value:(NSString *)value;
		[Export ("setDefaultHeader:value:")]
		void SetDefaultHeader (string header, string value);

 		// Sets the "Authorization" HTTP header set in request objects made by the HTTP client to a basic authentication value with Base64-encoded username and password. This overwrites any existing value for this header.
 		// @param username The HTTP basic auth username
 		// @param password The HTTP basic auth password
		// - (void)setAuthorizationHeaderWithUsername:(NSString *)username password:(NSString *)password;
		[Export ("setAuthorizationHeaderWithUsername:password:")]
		void SetAuthorizationHeaderWithUsername (string username, string password);

 		// Sets the "Authorization" HTTP header set in request objects made by the HTTP client to a token-based authentication value, such as an OAuth access token. This overwrites any existing value for this header.
 		// @param token The authentication token
		// - (void)setAuthorizationHeaderWithToken:(NSString *)token;
		[Export ("authorizationHeaderWithToken")]
		string AuthorizationHeaderWithToken { set; }

 		// Clears any existing value for the "Authorization" HTTP header.
		// - (void)clearAuthorizationHeader;
		[Export ("clearAuthorizationHeader")]
		void ClearAuthorizationHeader ();


 		// Creates an `NSMutableURLRequest` object with the specified HTTP method and path.
 		// If the HTTP method is `GET`, `HEAD`, or `DELETE`, the parameters will be used to construct a url-encoded query string that is appended to the request's URL. Otherwise, the parameters will be encoded according to the value of the `parameterEncoding` property, and set as the request body.
 		// @param method The HTTP method for the request, such as `GET`, `POST`, `PUT`, or `DELETE`. This parameter must not be `nil`.
 		// @param path The path to be appended to the HTTP client's base URL and used as the request URL. If `nil`, no path will be appended to the base URL.
 		// @param parameters The parameters to be either set as a query string for `GET` requests, or the request HTTP body.
 		// @return An `NSMutableURLRequest` object 
		// - (NSMutableURLRequest *)requestWithMethod:(NSString *)method 
		// 	path:(NSString *)path 
		//		parameters:(NSDictionary *)parameters;
		[Export ("requestWithMethod:path:parameters:")]
		NSMutableUrlRequest RequestWithMethod (string method, string path, NSDictionary parameters);

 		// Creates an `NSMutableURLRequest` object with the specified HTTP method and path, and constructs a `multipart/form-data` HTTP body, using the specified parameters and multipart form data block. See http://www.w3.org/TR/html4/interact/forms.html#h-17.13.4.2
 		// @param method The HTTP method for the request. This parameter must not be `GET` or `HEAD`, or `nil`.
 		// @param path The path to be appended to the HTTP client's base URL and used as the request URL.
 		// @param parameters The parameters to be encoded and set in the request HTTP body.
 		// @param block A block that takes a single argument and appends data to the HTTP body. The block argument is an object adopting the `AFMultipartFormData` protocol. This can be used to upload files, encode HTTP body as JSON or XML, or specify multiple values for the same parameter, as one might for array values.
 		// @discussion Multipart form requests are automatically streamed, reading files directly from disk along with in-memory data in a single HTTP body. The resulting `NSMutableURLRequest` object has an `HTTPBodyStream` property, so refrain from setting `HTTPBodyStream` or `HTTPBody` on this request object, as it will clear out the multipart form body stream.
 		// @return An `NSMutableURLRequest` object
		// - (NSMutableURLRequest *)multipartFormRequestWithMethod:(NSString *)method
		//	path:(NSString *)path
		//		parameters:(NSDictionary *)parameters
		//		constructingBodyWithBlock:(void (^)(id <AFMultipartFormData> formData))block;
		[Export ("multipartFormRequestWithMethod:path:parameters:constructingBodyWithBlock:")]
		NSMutableUrlRequest MultipartFormRequestWithMethod (string method, string path, NSDictionary parameters, Delegate block);

 		// Creates an `AFHTTPRequestOperation`.
 		// In order to determine what kind of operation is created, each registered subclass conforming to the `AFHTTPClient` protocol is consulted (in reverse order of when they were specified) to see if it can handle the specific request. The first class to return `YES` when sent a `canProcessRequest:` message is used to generate an operation using `HTTPRequestOperationWithRequest:success:failure:`.
 		// @param urlRequest The request object to be loaded asynchronously during execution of the operation.
 		// @param success A block object to be executed when the request operation finishes successfully. This block has no return value and takes two arguments: the created request operation and the object created from the response data of request.
 		// @param failure A block object to be executed when the request operation finishes unsuccessfully, or that finishes successfully, but encountered an error while parsing the response data. This block has no return value and takes two arguments:, the created request operation and the `NSError` object describing the network or parsing error that occurred.
		// - (AFHTTPRequestOperation *)HTTPRequestOperationWithRequest:(NSURLRequest *)urlRequest
		//	success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("HTTPRequestOperationWithRequest:success:failure:")]
		AFHTTPRequestOperation HTTPRequestOperationWithRequest (NSUrlRequest urlRequest, Delegate success);

 		// Enqueues an `AFHTTPRequestOperation` to the HTTP client's operation queue.
 		// @param operation The HTTP request operation to be enqueued.
		// - (void)enqueueHTTPRequestOperation:(AFHTTPRequestOperation *)operation;
		[Export ("enqueueHTTPRequestOperation:")]
		void EnqueueHTTPRequestOperation (AFHTTPRequestOperation operation);

 		// Cancels all operations in the HTTP client's operation queue whose URLs match the specified HTTP method and path.
 		// @param method The HTTP method to match for the cancelled requests, such as `GET`, `POST`, `PUT`, or `DELETE`. If `nil`, all request operations with URLs matching the path will be cancelled. 
 		// @param path The path appended to the HTTP client base URL to match against the cancelled requests. If `nil`, no path will be appended to the base URL.
 		// @discussion This method only cancels `AFHTTPRequestOperations` whose request URL matches the HTTP client base URL with the path appended. For complete control over the lifecycle of enqueued operations, you can access the `operationQueue` property directly, which allows you to, for instance, cancel operations filtered by a predicate, or simply use `-cancelAllRequests`. Note that the operation queue may include non-HTTP operations, so be sure to check the type before attempting to directly introspect an operation's `request` property.
		// - (void)cancelAllHTTPOperationsWithMethod:(NSString *)method path:(NSString *)path;
		[Export ("cancelAllHTTPOperationsWithMethod:path:")]
		void CancelAllHTTPOperationsWithMethod (string method, string path);

 		// Creates and enqueues an `AFHTTPRequestOperation` to the HTTP client's operation queue for each specified request object into a batch. When each request operation finishes, the specified progress block is executed, until all of the request operations have finished, at which point the completion block also executes.
 		// @param urlRequests The `NSURLRequest` objects used to create and enqueue operations.
 		// @param progressBlock A block object to be executed upon the completion of each request operation in the batch. This block has no return value and takes two arguments: the number of operations that have already finished execution, and the total number of operations.
 		// @param completionBlock A block object to be executed upon the completion of all of the request operations in the batch. This block has no return value and takes a single argument: the batched request operations. 
 		// @discussion Operations are created by passing the specified `NSURLRequest` objects in `requests`, using `-HTTPRequestOperationWithRequest:success:failure:`, with `nil` for both the `success` and `failure` parameters.
		// - (void)enqueueBatchOfHTTPRequestOperationsWithRequests:(NSArray *)urlRequests
		//	progressBlock:(void (^)(NSUInteger numberOfFinishedOperations, NSUInteger totalNumberOfOperations))progressBlock 
		//		completionBlock:(void (^)(NSArray *operations))completionBlock;
		[Export ("enqueueBatchOfHTTPRequestOperationsWithRequests:progressBlock:completionBlock:")]
		void EnqueueBatchOfHTTPRequestOperationsWithRequests (NSObject [] urlRequests, Delegate progressBlock);

 		// Enqueues the specified request operations into a batch. When each request operation finishes, the specified progress block is executed, until all of the request operations have finished, at which point the completion block also executes.
 		// @param operations The request operations used to be batched and enqueued.
 		// @param progressBlock A block object to be executed upon the completion of each request operation in the batch. This block has no return value and takes two arguments: the number of operations that have already finished execution, and the total number of operations.
 		// @param completionBlock A block object to be executed upon the completion of all of the request operations in the batch. This block has no return value and takes a single argument: the batched request operations. 
		// - (void)enqueueBatchOfHTTPRequestOperations:(NSArray *)operations 
		// 	progressBlock:(void (^)(NSUInteger numberOfFinishedOperations, NSUInteger totalNumberOfOperations))progressBlock 
		//		completionBlock:(void (^)(NSArray *operations))completionBlock;
		[Export ("enqueueBatchOfHTTPRequestOperations:progressBlock:completionBlock:")]
		void EnqueueBatchOfHTTPRequestOperations (NSObject [] operations, Delegate progressBlock);

 		// Creates an `AFHTTPRequestOperation` with a `GET` request, and enqueues it to the HTTP client's operation queue.
 		// @param path The path to be appended to the HTTP client's base URL and used as the request URL.
 		// @param parameters The parameters to be encoded and appended as the query string for the request URL.
 		// @param success A block object to be executed when the request operation finishes successfully. This block has no return value and takes two arguments: the created request operation and the object created from the response data of request.
 		// @param failure A block object to be executed when the request operation finishes unsuccessfully, or that finishes successfully, but encountered an error while parsing the response data. This block has no return value and takes two arguments:, the created request operation and the `NSError` object describing the network or parsing error that occurred.
 		// @see -HTTPRequestOperationWithRequest:success:failure:
		// - (void)getPath:(NSString *)path
		//	parameters:(NSDictionary *)parameters
		//		success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("getPath:parameters:success:failure:")]
		void GetPath (string path, NSDictionary parameters, Delegate success);

 		// Creates an `AFHTTPRequestOperation` with a `POST` request, and enqueues it to the HTTP client's operation queue.
 		// @param path The path to be appended to the HTTP client's base URL and used as the request URL.
 		// @param parameters The parameters to be encoded and set in the request HTTP body.
 		// @param success A block object to be executed when the request operation finishes successfully. This block has no return value and takes two arguments: the created request operation and the object created from the response data of request.
 		// @param failure A block object to be executed when the request operation finishes unsuccessfully, or that finishes successfully, but encountered an error while parsing the response data. This block has no return value and takes two arguments:, the created request operation and the `NSError` object describing the network or parsing error that occurred.
 		// @see -HTTPRequestOperationWithRequest:success:failure:
		// - (void)postPath:(NSString *)path 
		//	parameters:(NSDictionary *)parameters 
		//		success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("postPath:parameters:success:failure:")]
		void PostPath (string path, NSDictionary parameters, Delegate success);

 		// Creates an `AFHTTPRequestOperation` with a `PUT` request, and enqueues it to the HTTP client's operation queue.
 		// @param path The path to be appended to the HTTP client's base URL and used as the request URL.
 		// @param parameters The parameters to be encoded and set in the request HTTP body.
 		// @param success A block object to be executed when the request operation finishes successfully. This block has no return value and takes two arguments: the created request operation and the object created from the response data of request.
 		// @param failure A block object to be executed when the request operation finishes unsuccessfully, or that finishes successfully, but encountered an error while parsing the response data. This block has no return value and takes two arguments:, the created request operation and the `NSError` object describing the network or parsing error that occurred.
 		// @see -HTTPRequestOperationWithRequest:success:failure:
		// - (void)putPath:(NSString *)path 
		//	parameters:(NSDictionary *)parameters 
		//		success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("putPath:parameters:success:failure:")]
		void PutPath (string path, NSDictionary parameters, Delegate success);

 		// Creates an `AFHTTPRequestOperation` with a `DELETE` request, and enqueues it to the HTTP client's operation queue.
 		// @param path The path to be appended to the HTTP client's base URL and used as the request URL.
 		// @param parameters The parameters to be encoded and appended as the query string for the request URL.
 		// @param success A block object to be executed when the request operation finishes successfully. This block has no return value and takes two arguments: the created request operation and the object created from the response data of request.
 		// @param failure A block object to be executed when the request operation finishes unsuccessfully, or that finishes successfully, but encountered an error while parsing the response data. This block has no return value and takes two arguments:, the created request operation and the `NSError` object describing the network or parsing error that occurred.
 		// @see -HTTPRequestOperationWithRequest:success:failure:
		// - (void)deletePath:(NSString *)path 
		//	parameters:(NSDictionary *)parameters 
		//		success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("deletePath:parameters:success:failure:")]
		void DeletePath (string path, NSDictionary parameters, Delegate success);

 		// Creates an `AFHTTPRequestOperation` with a `PATCH` request, and enqueues it to the HTTP client's operation queue.
 		// @param path The path to be appended to the HTTP client's base URL and used as the request URL.
 		// @param parameters The parameters to be encoded and set in the request HTTP body.
 		// @param success A block object to be executed when the request operation finishes successfully. This block has no return value and takes two arguments: the created request operation and the object created from the response data of request.
 		// @param failure A block object to be executed when the request operation finishes unsuccessfully, or that finishes successfully, but encountered an error while parsing the response data. This block has no return value and takes two arguments:, the created request operation and the `NSError` object describing the network or parsing error that occurred.
 		// @see -HTTPRequestOperationWithRequest:success:failure:
		// - (void)patchPath:(NSString *)path
		//	parameters:(NSDictionary *)parameters 
		//		success:(void (^)(AFHTTPRequestOperation *operation, id responseObject))success
		//		failure:(void (^)(AFHTTPRequestOperation *operation, NSError *error))failure;
		[Export ("patchPath:parameters:success:failure:")]
		void PatchPath (string path, NSDictionary parameters, Delegate success);

		[Field ("kAFUploadStream3GSuggestedPacketSize")]
		uint KAFUploadStream3GSuggestedPacketSize { get; }

		[Field ("KAFUploadStream3GSuggestedDelay")]
		double kAFUploadStream3GSuggestedDelay { get; }
	}

	[Model]
	public partial interface AFMultipartFormData {

		[Export ("appendPartWithFileURL:name:error:")]
		bool AppendPartWithFileUrl (NSUrl fileURL, string name, out NSError error);

		[Export ("appendPartWithFileData:name:fileName:mimeType:")]
		void AppendPartWithFileData (NSData data, string name, string fileName, string mimeType);

		[Export ("appendPartWithFormData:name:")]
		void AppendPartWithFormData (NSData data, string name);

		[Export ("appendPartWithHeaders:body:")]
		void AppendPartWithHeaders (NSDictionary headers, NSData body);

		[Export ("throttleBandwidthWithPacketSize:delay:")]
		void ThrottleBandwidthWithPacketSize (uint numberOfBytes, double delay);
	}

	[BaseType (typeof (AFURLConnectionOperation))]
	public partial interface AFHTTPRequestOperation {

		[Export ("response")]
		NSHttpUrlResponse Response { get; }

		[Export ("hasAcceptableStatusCode")]
		bool HasAcceptableStatusCode { get; }

		[Export ("hasAcceptableContentType")]
		bool HasAcceptableContentType { get; }

		[Export ("successCallbackQueue")]
		DispatchQueue SuccessCallbackQueue { get; set; }

		[Export ("failureCallbackQueue")]
		DispatchQueue FailureCallbackQueue { get; set; }

		[Export ("acceptableStatusCodes")]
		NSIndexSet AcceptableStatusCodes { get; }

		[Static, Export ("addAcceptableStatusCodes:")]
		void AddAcceptableStatusCodes (NSIndexSet statusCodes);

		[Export ("acceptableContentTypes")]
		NSSet AcceptableContentTypes { get; }

		[Static, Export ("addAcceptableContentTypes:")]
		void AddAcceptableContentTypes (NSSet contentTypes);

		[Static, Export ("canProcessRequest:")]
		bool CanProcessRequest (NSUrlRequest urlRequest);

		[Export ("completionBlockWithSuccess:failure")]
		Delegate CompletionBlockWithSuccess { set; }
	}

	[BaseType (typeof (AFHTTPRequestOperation))]
	public partial interface AFImageRequestOperation {

		[Export ("responseImage")]
		UIImage ResponseImage { get; }

		[Export ("imageScale")]
		float ImageScale { get; set; }

		// TODO
		//		[Static, Export ("imageRequestOperationWithRequest:success:")]
		//		AFImageRequestOperation ImageRequestOperationWithRequest (NSUrlRequest urlRequest, Delegate success);
		//
		//		[Static, Export ("imageRequestOperationWithRequest:imageProcessingBlock:success:failure:")]
		//		AFImageRequestOperation ImageRequestOperationWithRequest (NSUrlRequest urlRequest, Delegate imageProcessingBlock);
	}

	[BaseType (typeof (AFHTTPRequestOperation))]
	public partial interface AFJSONRequestOperation {

		[Export ("responseJSON")]
		NSObject ResponseJson { get; }

		[Export ("JSONReadingOptions")]
		NSJsonReadingOptions JsonrEadingOptions { get; set; }

		[Static, Export ("JSONRequestOperationWithRequest:success:failure:")]
		AFJSONRequestOperation JsonrEquestOperationWithRequest (NSUrlRequest urlRequest, Delegate success);
	}

	[BaseType (typeof (NSObject))]
	public partial interface AFNetworkActivityIndicatorManager {

		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		[Export ("isNetworkActivityIndicatorVisible")]
		bool IsNetworkActivityIndicatorVisible { get; }

		[Export ("sharedManager")]
		AFNetworkActivityIndicatorManager SharedManager { get; }

		[Export ("incrementActivityCount")]
		void IncrementActivityCount ();

		[Export ("decrementActivityCount")]
		void DecrementActivityCount ();
	}

	[BaseType (typeof (AFHTTPRequestOperation))]
	public partial interface AFPropertyListRequestOperation {

		[Export ("responsePropertyList")]
		NSObject ResponsePropertyList { get; }

		[Export ("propertyListReadOptions")]
		NSPropertyListReadOptions PropertyListReadOptions { get; set; }

		[Static, Export ("propertyListRequestOperationWithRequest:success:failure:")]
		AFPropertyListRequestOperation PropertyListRequestOperationWithRequest (NSUrlRequest urlRequest, Delegate success);
	}

	[BaseType (typeof (NSOperation))]
	public partial interface AFURLConnectionOperation {

		[Export ("runLoopModes")]
		NSSet RunLoopModes { get; set; }

		[Export ("request")]
		NSUrlRequest Request { get; }

		[Export ("response")]
		NSUrlResponse Response { get; }

		[Export ("error")]
		NSError Error { get; }

		[Export ("responseData")]
		NSData ResponseData { get; }

		[Export ("responseString")]
		string ResponseString { get; }

		[Export ("inputStream")]
		NSInputStream InputStream { get; set; }

		[Export ("outputStream")]
		NSOutputStream OutputStream { get; set; }

		[Export ("initWithRequest:")]
		IntPtr Constructor (NSUrlRequest urlRequest);

		[Export ("pause")]
		void Pause ();

		[Export ("isPaused")]
		bool IsPaused { get; }

		[Export ("resume")]
		void Resume ();

		[Export ("shouldExecuteAsBackgroundTaskWithExpirationHandler")]
		Delegate ShouldExecuteAsBackgroundTaskWithExpirationHandler { set; }
	}

	#endregion

	#region Core

	//typedef void (^NIOperationBlock)(NIOperation* operation);
	public delegate void NIOperationBlock(NIOperation operation);
	//typedef void (^NIOperationDidFailBlock)(NIOperation* operation, NSError* error);
	public delegate void NIOperationDidFailBlock(NIOperation operation, NSError error);

	[BaseType (typeof (NSOperation))]
	public partial interface NIOperation {
		
		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		NIOperationDelegate Delegate { get; set; }

		[Export ("lastError")]
		NSError LastError { get; }
		
		[Export ("tag")]
		int Tag { get; set; }
		
		[Export ("didStartBlock")]
		NIOperationBlock DidStartBlock { get; set; }
		
		[Export ("didFinishBlock")]
		NIOperationBlock DidFinishBlock { get; set; }
		
		[Export ("didFailWithErrorBlock")]
		NIOperationDidFailBlock DidFailWithErrorBlock { get; set; }
		
		[Export ("willFinishBlock")]
		NIOperationBlock WillFinishBlock { get; set; }
		
		[Export ("didStart")]
		void DidStart ();
		
		[Export ("didFinish")]
		void DidFinish ();
		
		[Export ("didFailWithError:")]
		void DidFailWithError (NSError error);
		
		[Export ("willFinish")]
		void WillFinish ();
	}
	
	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NIOperationDelegate {
		
		[Export ("nimbusOperationDidStart:")]
		void NimbusOperationDidStart (NIOperation operation);
		
		[Export ("nimbusOperationWillFinish:")]
		void NimbusOperationWillFinish (NIOperation operation);
		
		[Export ("nimbusOperationDidFinish:")]
		void NimbusOperationDidFinish (NIOperation operation);
		
		[Export ("nimbusOperationDidFail:withError:")]
		void NimbusOperationDidFail (NIOperation operation, NSError error);
	}

	#endregion

	#region NetworkImage

	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NINetworkImageOperation {
		
		[Export ("cacheIdentifier")]
		string CacheIdentifier { get; }
		
		[Export ("imageCropRect")]
		RectangleF ImageCropRect { get; set; }
		
		[Export ("imageDisplaySize")]
		SizeF ImageDisplaySize { get; set; }
		
		[Export ("scaleOptions")]
		NINetworkImageViewScaleOptions ScaleOptions { get; set; }
		
		[Export ("interpolationQuality")]
		CGInterpolationQuality InterpolationQuality { get; set; }
		
		[Export ("imageContentMode")]
		UIViewContentMode ImageContentMode { get; set; }
		
		[Export ("imageCroppedAndSizedForDisplay")]
		UIImage ImageCroppedAndSizedForDisplay { get; set; }
	}

	[BaseType (typeof (UIImageView))]
	public partial interface NINetworkImageView : NIOperationDelegate {

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap("WeakDelegate")]
		NINetworkImageViewDelegate Delegate { get; set; }

		[Export ("initWithImage:")]
		IntPtr Constructor (UIImage image);
		
		[Export ("initialImage")]
		UIImage InitialImage { get; set; }
		
		[Export ("sizeForDisplay")]
		bool SizeForDisplay { get; set; }
		
		[Export ("scaleOptions")]
		NINetworkImageViewScaleOptions ScaleOptions { get; set; }
		
		[Export ("interpolationQuality")]
		CGInterpolationQuality InterpolationQuality { get; set; }
		
//		[Export ("imageMemoryCache")]
//		NIImageMemoryCache ImageMemoryCache { get; set; }
		
		[Export ("networkOperationQueue")]
		NSOperationQueue NetworkOperationQueue { get; set; }
		
		[Export ("maxAge")]
		double MaxAge { get; set; }
		
		[Export ("pathToNetworkImage")]
		string PathToNetworkImage { set; }
		
		[Export ("setPathToNetworkImage:forDisplaySize:")]
		void SetPathToNetworkImage (string pathToNetworkImage, SizeF displaySize);

		[Export ("setPathToNetworkImage:forDisplaySize:contentMode:")]
		void SetPathToNetworkImage (string pathToNetworkImage, SizeF displaySize, UIViewContentMode contentMode);
		
		[Export ("setPathToNetworkImage:forDisplaySize:contentMode:cropRect:")]
		void SetPathToNetworkImage (string pathToNetworkImage, SizeF displaySize, UIViewContentMode contentMode, RectangleF cropRect);
		
		[Export ("setPathToNetworkImage:cropRect:")]
		void SetPathToNetworkImage (string pathToNetworkImage, RectangleF cropRect);
		
		[Export ("setPathToNetworkImage:contentMode:")]
		void SetPathToNetworkImage (string pathToNetworkImage, UIViewContentMode contentMode);
		
		[Export ("setNetworkImageOperation:forDisplaySize:contentMode:cropRect:")]
		void SetNetworkImageOperation (NINetworkImageOperation operation, SizeF displaySize, UIViewContentMode contentMode, RectangleF cropRect);
		
		[Export ("loading")]
		bool Loading { [Bind ("isLoading")] get; }
		
		[Export ("prepareForReuse")]
		void PrepareForReuse ();
		
		[Export ("networkImageViewDidStartLoading")]
		void NetworkImageViewDidStartLoading ();
		
		[Export ("networkImageViewDidLoadImage:")]
		void NetworkImageViewDidLoadImage (UIImage image);
		
		[Export ("networkImageViewDidFailWithError:")]
		void NetworkImageViewDidFailWithError (NSError error);
	}

	[Model]
	[BaseType(typeof(NSObject))]
	public partial interface NINetworkImageViewDelegate  {
		
		[Export ("networkImageViewDidStartLoad:")]
		void NetworkImageViewDidStartLoad (NINetworkImageView imageView);
		
		[Export ("networkImageView:didLoadImage:")]
		void NetworkImageViewDidLoadImage (NINetworkImageView imageView, UIImage image);
		
		[Export ("networkImageView:didFailWithError:")]
		void NetworkImageViewDidFailWithError (NINetworkImageView imageView, NSError error);
		
		[Export ("networkImageView:readBytes:totalBytes:")]
		void NetworkImageViewReadBytes (NINetworkImageView imageView, long readBytes, long totalBytes);
	}

	#endregion
}


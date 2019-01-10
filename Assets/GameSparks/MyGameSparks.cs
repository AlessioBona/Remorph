#pragma warning disable 612,618
#pragma warning disable 0114
#pragma warning disable 0108

using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
		public class LogEventRequest_STORE_CUSTOM_LEVEL : GSTypedRequest<LogEventRequest_STORE_CUSTOM_LEVEL, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_STORE_CUSTOM_LEVEL() : base("LogEventRequest"){
			request.AddString("eventKey", "STORE_CUSTOM_LEVEL");
		}
		public LogEventRequest_STORE_CUSTOM_LEVEL Set_LEVEL_DATA( GSData value )
		{
			request.AddObject("LEVEL_DATA", value);
			return this;
		}			
		
		public LogEventRequest_STORE_CUSTOM_LEVEL Set_LEVEL_NAME( string value )
		{
			request.AddString("LEVEL_NAME", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_STORE_CUSTOM_LEVEL : GSTypedRequest<LogChallengeEventRequest_STORE_CUSTOM_LEVEL, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_STORE_CUSTOM_LEVEL() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "STORE_CUSTOM_LEVEL");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_STORE_CUSTOM_LEVEL SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_STORE_CUSTOM_LEVEL Set_LEVEL_DATA( GSData value )
		{
			request.AddObject("LEVEL_DATA", value);
			return this;
		}
		
		public LogChallengeEventRequest_STORE_CUSTOM_LEVEL Set_LEVEL_NAME( string value )
		{
			request.AddString("LEVEL_NAME", value);
			return this;
		}
	}
	
}
	

namespace GameSparks.Api.Messages {


}

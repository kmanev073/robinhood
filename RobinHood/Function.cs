using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace RobinHood
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SsmlOutputSpeech innerResponse = new SsmlOutputSpeech();
            

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                innerResponse.Ssml = "<speak>You can't open this skill.</speak>";
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;
                switch (intentRequest.Intent.Name)
                {
                    case "GetBitcoinPrice":
                        StandardHttpClient httpClient = new StandardHttpClient();
                        string coindeskPrice = await httpClient.GetAsync("http://api.coindesk.com/site/chartandheaderdata?currency=BTC");
                        int patternIndex = coindeskPrice.IndexOf("{\"USD\":{\"symbol\":\"&#36;\",\"rate_float\":"),
                            patternLength = ("{\"USD\":{\"symbol\":\"&#36;\",\"rate_float\":").Length,
                            endPatternIndex = coindeskPrice.IndexOf("},\"", patternIndex + patternLength);
                        float bitcoinPrice = float.Parse(coindeskPrice.Substring(patternIndex + patternLength, endPatternIndex - patternIndex - patternLength));
                        innerResponse.Ssml = $"<speak>One Bitcoin costs <say-as interpret-as=\"unit\">USD{Math.Round(bitcoinPrice,2)}</say-as>.</speak>";
                        break;
                    default:
                        innerResponse.Ssml = $"<speak>An error has occurred!</speak>";
                        break;
                }
            }

            return ResponseBuilder.Tell(innerResponse);
        }
    }
}

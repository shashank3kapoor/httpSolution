using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HttpSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HttpSolution.Controllers {
	[ApiController]
	[Route ("[controller]")]
	public class HttpClientController : ControllerBase {
		
		private readonly ILogger<HttpClientController> _logger;
		private readonly IConfiguration _config;

		private string apiGoogleAddress;
		private string coordsGooglePath;
		private string keyGoogleAddress;

		public HttpClientController (ILogger<HttpClientController> logger
				, IConfiguration config
			) {
			_logger = logger;
			_config = config;
			GoogleApi apiDetails = _config.GetSection ("googleApi").Get<GoogleApi> ();
			this.apiGoogleAddress = apiDetails.apiGoogleAddress;
			this.coordsGooglePath = apiDetails.coordsGooglePath;
			this.keyGoogleAddress = apiDetails.keyGoogleAddress;
		}

		private HttpClient getGoogleClient () {
			try {

				HttpClient httpClient = new HttpClient ();

				httpClient.DefaultRequestHeaders.Accept.Clear ();

				ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

				return httpClient;
			} catch (Exception e) {
				throw e;
			}
		}

		[HttpPost]
		public async Task<HttpResponseMessage> GetGoogleCords ([FromBody] Site site) {
			try {
				if (string.IsNullOrEmpty (site.site) == true) {
					throw new Exception ("Site is required.");
				} else {
					HttpClient httpClient = this.getGoogleClient ();
					site.site = site.site.Replace (" ", "+");
					string gURL = "https://" + this.apiGoogleAddress + this.coordsGooglePath + "?address=" + site.site + "&key=" + this.keyGoogleAddress;
					var response = await httpClient.GetAsync (gURL);

					return response;
				}

			} catch (Exception e) {
				throw e;
			}
		}

		/*
		 * Test Api is started
		 */
		[HttpGet]
		public string Get ()
		{
			return "pong";
		}

		/*[HttpGet, Route("/{d}")]
		public string GetVal ([FromBody]string d)
		{
			return "pong" + d;
		}*/
	}
}

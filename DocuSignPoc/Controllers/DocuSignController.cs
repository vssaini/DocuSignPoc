using DocuSignPoc.Contracts;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DocuSignPoc.Controllers
{
    public class DocuSignController : Controller
    {
        private readonly IDocuSignService _dsService;
        private readonly ILogService _logService;

        public DocuSignController(ILogService logService, IDocuSignService dsService)
        {
            _dsService = dsService;
            _logService = logService;
        }

        public ActionResult Index(bool? consentGranted)
        {
            // For DocuSign configuration visit https://admindemo.docusign.com/

            if (consentGranted.HasValue)
            {
                ViewBag.ConsentGranted = consentGranted.Value;
                return View();
            }

            ViewBag.ConsentGranted = _dsService.IsConsentGranted();
            return View();
        }

        [HttpPost]
        public ActionResult GrantConsent()
        {
            try
            {
                var url = _dsService.GetConsentUrl();
                return Json(new { status = false, url, message = "Redirecting to consent URL" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult Callback()
        {
            return RedirectToAction("Index", new { consentGranted = true });
        }

        [HttpPost]
        [Route("Webhook")]
        [AllowAnonymous]
        public async Task Webhook()
        {
            var request = Request;
            var signature = request.Headers["X-Docusign-Signature-1"];

            request.InputStream.Position = 0;
            using (var streamReader = new StreamReader(request.InputStream))
            {
                var payload = await streamReader.ReadToEndAsync();

                try
                {
                    _dsService.ValidateRequest(payload, signature);
                    _dsService.SaveSignedFile(payload);
                }
                catch (Exception e)
                {
                    _logService.LogError(e, "Error occurred in Webhook", payload);
                }
            }
        }
    }
}
using BAL.Services;
using BAL.ViewModel;
using System.Web.Mvc;
using System.Collections.Generic;
using BOL.Constant;

namespace ParkingManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly VehicleRegistrationService _vehicleRegistrationService;
        private readonly BlockService _blockService;
        private readonly VehicleRegistrationService _vehicleService;

        public HomeController()
        {
            _vehicleRegistrationService = new VehicleRegistrationService();
            _blockService = new BlockService();
            _vehicleService = new VehicleRegistrationService();
        }

        // GET: Home
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "ParkingAllotment");
            }

            IEnumerable<BlockViewModel> blocks = _blockService.GetAllBlock();
            IEnumerable<VehicleRegistrationViewModel> vehicle = _vehicleService.GetAllVehicle();

            ViewBag.Blocks = new SelectList(blocks, "BlockNo", "BlockNo");
            ViewBag.Vehicles = new SelectList(vehicle, "VehicleRCNo", "VehicleRCNo");

            return View();
        }

        [HttpPost]
        public JsonResult LoadVehicleData(DataTableAjaxPostModel model, string filterVehicle)
        {
            object vehicleData = _vehicleRegistrationService.GetRegisterVehicleData(model, filterVehicle);
            return Json(vehicleData);
        }

        [HttpPost]
        public ActionResult GetParkedVehicles(DataTableAjaxPostModel model, string blockNo)
        {
            object parkedVehicles = _vehicleRegistrationService.GetParkedVehicles(model, blockNo);
            return Json(parkedVehicles);
        }

        [HttpPost]
        public JsonResult Create(VehicleRegistrationCreateModel registrationModel)
        {
            if (ModelState.IsValid && !_vehicleRegistrationService.CheckUniqueRcNo(registrationModel.VehicleRCNo))
            {
                _vehicleRegistrationService.RegisterVehicle(registrationModel);
                return Json(VehicleRegistrationConstant.Success, JsonRequestBehavior.AllowGet);
            }

            return Json(VehicleRegistrationConstant.Unsuccess, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRegistrationDataById(int id)
        {
            VehicleRegistrationViewModel vehicleData = _vehicleRegistrationService.GetVehicleById(id);
            return Json(vehicleData);
        }

        // POST: Home/Edit/model
        [HttpPost]
        public JsonResult EditVehicle(VehicleRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                _vehicleRegistrationService.UpdateVehicleRegistration(model);
                return Json(VehicleRegistrationConstant.Success, JsonRequestBehavior.AllowGet);
            }
            return Json(VehicleRegistrationConstant.Unsuccess, JsonRequestBehavior.AllowGet);
        }

        // GET: Home/Delete/1
        public ActionResult Delete(int id)
        {
            VehicleRegistrationViewModel model = _vehicleRegistrationService.GetVehicleById(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            _vehicleRegistrationService.DeleteRegisterVehicle(id);
            return RedirectToAction("Index");
        }


        public ActionResult AvailableBlock()
        {
            IEnumerable<BlockViewModel> block = _blockService.GetAllBlock();
            return PartialView("_AvailableBlock", block);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vehicleRegistrationService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
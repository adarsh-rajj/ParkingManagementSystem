using BAL.Services;
using BAL.ViewModel;
using BOL.Constant;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ParkingManagementSystem.Controllers
{
    public class ParkingAllotmentController : Controller
    {
        private readonly ParkingAllocationService _parkingService;
        private readonly BlockService _blockService;
        private readonly VehicleRegistrationService _vehicleService;

        public ParkingAllotmentController()
        {
            _parkingService = new ParkingAllocationService();
            _blockService = new BlockService();
            _vehicleService = new VehicleRegistrationService();
        }

        // GET: Parking
        public ActionResult Index()
        {
            IEnumerable<BlockViewModel> block = _blockService.GetAllBlock();
            IEnumerable<VehicleRegistrationViewModel> vehicle = _vehicleService.GetAllUnParkedVehicle();

            ViewBag.Blocks = new SelectList(block, "BlockNo", "BlockNo");
            ViewBag.Vehicles = new SelectList(vehicle, "VehicleRcNoId", "VehicleRcNoId");

            return View();
        }


        [HttpPost]
        public JsonResult LoadParkingData(DataTableAjaxPostModel model, string blockNo)
        {
            object parkingData = _parkingService.GetParkingAllocationData(model, blockNo);
            return Json(parkingData);
        }


        [HttpPost]
        public JsonResult Create(ParkingCreateViewModel parkingCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(VehicleRegistrationConstant.Unsuccess, JsonRequestBehavior.AllowGet);
            }
            _parkingService.AddParking(parkingCreateViewModel);
            return Json(VehicleRegistrationConstant.Success, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetParkingDateById(int id)
        {
            ParkingAllocationViewModel parkingAllocation = _parkingService.GetAllocationById(id);

            ParkingAllocationViewModel parkingModel = new ParkingAllocationViewModel()
            {
                Blocks = _blockService.GetAllBlock(),
                AllocationId = parkingAllocation.AllocationId,
                BlockNo = parkingAllocation.BlockNo,
                VehicleRcNoId = parkingAllocation.VehicleRcNoId,
                Description = parkingAllocation.Description,
                ParkingDateFrom = parkingAllocation.ParkingDateFrom,
                ParkingDateTo = parkingAllocation.ParkingDateTo,
                CreatedDate = parkingAllocation.CreatedDate,
                ModifiedDate = parkingAllocation.ModifiedDate
            };
            return Json(parkingModel, JsonRequestBehavior.AllowGet);
        }

        // POST: ParkingAllotment/Edit/model
        [HttpPost]
        public JsonResult EditParking(ParkingAllocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                _parkingService.UpdateParkingAllocation(model);
                return Json(VehicleRegistrationConstant.Success, JsonRequestBehavior.AllowGet);
            }
            return Json(VehicleRegistrationConstant.Unsuccess, JsonRequestBehavior.AllowGet);
        }


        // GET: ParkingAllotment/Delete/1
        public ActionResult Delete(int id)
        {
            ParkingAllocationViewModel model = _parkingService.GetAllocationById(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            _parkingService.DeleteParkingAllocation(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _parkingService.Dispose();  
            }
            base.Dispose(disposing);
        }
    }
}
using _ExcellOn_.Areas.Admin.Model;
using _ExcellOn_.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _ExcellOn_.Areas.Admin.Controllers
{
    public class ServiceManagementController : BaseController
    {
        private Entities db = new Entities();
        // GET: Admin/ServiceManagement

        [HasPermission(Permission = "Service_List" )]
        public ActionResult ServiceIndex()
        {
            var list_service = db.Services.ToList();
            ViewBag.list_service = list_service;
            return View();

        }
        [HasPermission(Permission = "Service_List")]
        public ActionResult ServiceIndex2()
        {
            List<Service> list_service = db.Services.ToList();
            return View(list_service);

        }

        [HasPermission(Permission = "Service_List")]
        [HttpGet]
        public JsonResult GetOrderDetail(int Service_ID)
        {

            List<OrderDetail> list_ord = db.OrderDetails.Where(x => x.ServiceId == Service_ID).ToList();
            string json_convert = JsonConvert.SerializeObject(list_ord);
            return Json(json_convert, JsonRequestBehavior.AllowGet);

        }

        [HasPermission(Permission = "Service_Add")]
        public ActionResult Add()
        {

            return View();


        }
        [HasPermission(Permission = "Service_Add")]
        public ActionResult Add_Submit(Service request, HttpPostedFileBase FeatureImage, HttpPostedFileBase[] Service_AdditionalImage)
        {

            // Create Service and add Feature Image
            Service newService = new Service();
            newService.Service_Description = request.Service_Description;
            newService.Service_Name = request.Service_Name;
            newService.Service_Price = request.Service_Price;
            newService.Service_PriceSale = request.Service_PriceSale;
            newService.Service_SaleStatus = request.Service_SaleStatus;
            if (FeatureImage != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(FeatureImage.FileName);
                string Extension = Path.GetExtension(FeatureImage.FileName);
                FileName = FileName + Extension;
                newService.Service_Image = "/Public/Image/" + FileName;

                FeatureImage.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
            }
            db.Services.Add(newService);

            // Add Image for Service

            foreach (HttpPostedFileBase file in Service_AdditionalImage)
            {
                //Checking file is available to save.  
                if (file != null)
                {
                    Image newImage = new Image();
                    string filename = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    filename = filename + extension;

                    newImage.Image_link = "/Public/Image/" + filename;
                    file.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), filename));
                    newImage.ServiceId = newService.Id;
                    db.Images.Add(newImage);
                }
            }
            db.SaveChanges();
            return RedirectToAction("ServiceIndex");


        }
        [HasPermission(Permission = "Service_Edit")]
        public ActionResult Edit_Submit(Service request, HttpPostedFileBase FeatureImage, HttpPostedFileBase[] Service_AdditionalImage)
        {

            // Create Service and add Feature Image
            Service service = db.Services.Where(x => x.Id == request.Id).FirstOrDefault();
            if (service != null)
            {
                if (Service_AdditionalImage[0] == null)
                {

                }
                else
                {
                    //Remove all Image Detail in table Image of current Service after update image
                    List<Image> list_image = db.Images.Where(x => x.ServiceId == service.Id).ToList();
                    foreach (var item in list_image)
                    {
                        db.Images.Remove(item);
                    }
                    // Add Image for Service
                    foreach (HttpPostedFileBase file in Service_AdditionalImage)
                    {
                        //Checking file is available to save.  
                        if (file != null)
                        {
                            Image newImage = new Image();
                            string filename = Path.GetFileNameWithoutExtension(file.FileName);
                            string extension = Path.GetExtension(file.FileName);
                            filename = filename + extension;

                            newImage.Image_link = "/Public/Image/" + filename;
                            file.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), filename));
                            newImage.ServiceId = service.Id;
                            db.Images.Add(newImage);
                        }
                    }
                }

                service.Service_Description = request.Service_Description;
                service.Service_Name = request.Service_Name;
                service.Service_Price = request.Service_Price;
                service.Service_PriceSale = request.Service_PriceSale;
                service.Service_SaleStatus = request.Service_SaleStatus;
                if (FeatureImage != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(FeatureImage.FileName);
                    string Extension = Path.GetExtension(FeatureImage.FileName);
                    FileName = FileName + Extension;
                    service.Service_Image = "/Public/Image/" + FileName;

                    FeatureImage.SaveAs(Path.Combine(Server.MapPath("/Public/Image/"), FileName));
                }

                db.SaveChanges();
                return RedirectToAction("ServiceIndex");
            }
            else
            {
                return RedirectToAction("ServiceIndex");
            }



        }
        [HasPermission(Permission = "Service_Edit")]
        public ActionResult Edit(int id)
        {
            Service edit_service = db.Services.Where(x => x.Id == id).FirstOrDefault();
            return View(edit_service);
        }

        [HasPermission(Permission = "Service_Delete")]
        [HttpGet]
        public JsonResult Delete(int ServiceId)
        {

            Service service = db.Services.Where(x => x.Id == ServiceId).FirstOrDefault();
            List<Image> service_image = db.Images.Where(x => x.ServiceId == ServiceId).ToList();
            foreach (var item in service_image)
            {
                db.Images.Remove(item);
            }
            db.Services.Remove(service);
            db.SaveChanges();
            return Json("/Admin/ServiceManagement/ServiceIndex", JsonRequestBehavior.AllowGet);

        }
    }
}
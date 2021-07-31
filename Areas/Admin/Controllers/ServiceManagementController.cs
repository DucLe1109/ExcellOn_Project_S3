using _ExcellOn_.Models;
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
        public ActionResult ServiceIndex()
        {
            Session["return_url"] = "/Admin/ServiceManagement/ServiceIndex";
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;

                var list_service = db.Services.ToList();
                ViewBag.list_service = list_service;
                return View();
            }
            else
            {
                return Redirect("/AdminLogin");
            }
        }

        public ActionResult Add()
        {
            Session["return_url"] = "/Admin/ServiceManagement/Add";
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;
                return View();
            }
            else
            {
                return Redirect("/AdminLogin");
            }
           
        }

        public ActionResult Add_Submit(Service request, HttpPostedFileBase FeatureImage, HttpPostedFileBase[] Service_AdditionalImage)
        {
            Session["return_url"] = "/Admin/ServiceManagement/Add_Submit";
            if (check_auth())
            {
                // Create Service and add Feature Image
                Service newService = new Service();
                newService.Service_Description = request.Service_Description;
                newService.Service_Name = request.Service_Name;
                newService.Service_Price = request.Service_Price;
                newService.Service_PriceSale = request.Service_PriceSale;
                newService.Service_SaleStatus = request.Service_SaleStatus;
                newService.Service_TaxPercentage = request.Service_TaxPercentage;
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
            else
            {
                return Redirect("/AdminLogin");
            }
           
        }

        public ActionResult Edit_Submit(Service request, HttpPostedFileBase FeatureImage, HttpPostedFileBase[] Service_AdditionalImage)
        {
            Session["return_url"] = "/Admin/ServiceManagement/Edit_Submit";
            if (check_auth())
            {
                // Create Service and add Feature Image
                Service service = db.Services.Where(x=> x.Id == request.Id).FirstOrDefault();
                if (service != null)
                {
                    if (Service_AdditionalImage[0] == null)
                    {
                        
                    }
                    else
                    {
                        //Remove all Image Detail in table Image of current Service after update image
                        List<Image> list_image = db.Images.Where(x=> x.ServiceId == service.Id).ToList();
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
                    service.Service_TaxPercentage = request.Service_TaxPercentage;
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
            else
            {
                return Redirect("/AdminLogin");
            }

        }

        public ActionResult Edit(int id)
        {
            Session["return_url"] = "/Admin/ServiceManagement/Edit/" + id;
            if (check_auth())
            {
                string UserName = (string)Session["UserName"];
                var CurrentUser = db.UserInFoes.Where(x => x.User_Name == UserName).FirstOrDefault();
                ViewBag.CurrentUser = CurrentUser;

                Service edit_service = db.Services.Where(x => x.Id == id).FirstOrDefault() ;
                return View(edit_service);
            }
            else
            {
                return Redirect("/AdminLogin");
            }

        }

        [HttpGet]
        public JsonResult Delete(int ServiceId)
        {
            Session["return_url"] = "/Admin/ServiceManagement/Delete?ServiceId=" + ServiceId;
            if (check_auth())
            {
                Service service = db.Services.Where(x => x.Id == ServiceId).FirstOrDefault();
                List<Image> service_image = db.Images.Where(x=> x.ServiceId == ServiceId).ToList();
                foreach (var item in service_image)
                {
                    db.Images.Remove(item);
                }
                db.Services.Remove(service);
                db.SaveChanges();
                return Json("/Admin/ServiceManagement/ServiceIndex", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("/Admin/User/Login", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
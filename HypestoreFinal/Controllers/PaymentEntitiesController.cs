using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Util;
using HypestoreFinal.Models;

using HypestoreFinal.ViewModels;

namespace MontclairStore.Controllers
{
    public class PaymentEntitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PaymentEntities
        public ActionResult Index()
        {
            var payments = db.Payments.Include(p => p.Customer);
            return View(payments.ToList());
        }

        // GET: PaymentEntities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment paymentEntity = db.Payments.Find(id);
            if (paymentEntity == null)
            {
                return HttpNotFound();
            }
            return View(paymentEntity);
        }

        // GET: PaymentEntities/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
            return View();
        }

        // POST: PaymentEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "payment_ID,CustomerId,Email,Date,AmountPaid,PaymentFor,PaymentMethod")] Payment paymentEntity)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(paymentEntity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", paymentEntity.CustomerId);
            return View(paymentEntity);
        }
        //public ActionResult Payment(int vid)
        //{
        //    //var petv = db.PetVisits.Find(vid);

        //    //ViewBag.Prescription = db.Prescriptions.ToList().FindAll(x => x.VisitId == petv.VisitId);
        //    //ViewBag.Total = PrescriptionTotal(vid);
        //    //ViewBag.VisitProcedure = db.VisitProcedures.ToList().FindAll(x => x.PetVisitID == petv.VisitId);
        //    //var ProcedureCost = db.VisitProcedures.ToList().Find(x => x.PetVisitID == petv.VisitId);
        //    //var procedure = db.VisitProcedures.ToList().FindAll(x => x.PetVisitID == vid);
        //    //var pre = db.Prescriptions.Find(vid);
        //    Payment_VM vm = new Payment_VM();

        //    vm.AmountPaid = PrescriptionTotal(vid);
            //vm.PrimaryDiagnosis = petv.PrimaryDiagnosis;
            
            //vm.VisitId = petv.VisitId;
            //vm.RefNo = petv.RefNo;


            //try
            //{

            //    string table = "<br/>" +
            //                   "Prescription in this order<br/>" +
            //                   "<table>";
            //    table += "<tr>"
            //             //"<th>MedName</th>"
            //             +
            //              "<th>Duration</th>"
            //              +
            //               "<th>isBeforeMeal</th>"
            //               +
            //             "<th> ExtraNotes</th>"
            //             +
            //              "<th>Price</th>"
            //              +
            //             "<th>Qty</th>" +
            //             "</tr>";
        //        foreach (var dosage in (List<Prescription>)ViewBag.Prescription)
        //        {
        //            string dosagesinoder = "<tr> " +
        //                                 // "<td>" + dosage.MedName + " </td>" +
        //                                 "<td>" + dosage.Duration + " </td>" +
        //                                 "<td> " + dosage.isBeforeMeal + " </td>" +
        //                                 "<td> " + dosage.ExtraNotes + " </td>" +
        //                                 "<td>R " + dosage.Price + " </td>" +
        //                                 "<td> " + dosage.Qty + " </td>" +
        //                                 "<tr/>";
        //            table += dosagesinoder;
        //        }

        //        table += "<tr>" +
        //                 "<th></th>"
        //                 +
        //                 "<th></th>"
        //                 +
        //             //"<th>" + db.Procedures.ToList().Find(x => x.ProCode == proCode).ProCost.ToString("R0.00") + "</th>" +
        //             // "<th>" + db.OvernightAdmissions.ToList().Find(x => x.OvernightAdmissionId == AdmCost).calcFinalCost().ToString("") +"</th>"+
        //             "<th>" + ProcedureCost + "" + "</th>" +
        //                 "</tr>";
        //        table += "</table>";


        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    try
        //    {
        //        string table = "<br/>" +
        //                                       "Procedure in this order<br/>" +
        //                                       "<table>";
        //        table += "<tr>" 
        //                 +
        //                  "<th>VPID</th>"
        //                  +
        //                   "<th>DatePeformed</th>"
        //                   +
        //                 "<th> Reaction</th>"
        //                 +
        //                  "<th>ExtraNotes</th>"
        //                  +
        //                 "</tr>";
        //        foreach (var dosage in (List<VisitProcedure>)ViewBag.VisitProcedure)
        //        {
        //            string dosagesinoder = "<tr> " +
        //                                "<td>" + dosage.VPID + " </td>" +
        //                                 "<td> " + dosage.DatePeformed + " </td>" +
        //                                 "<td> " + dosage.Reaction + " </td>" +
        //                                  "<td> " + dosage.ExtraNotes + " </td>" +
                                        
        //                                 "<tr/>";
        //            table += dosagesinoder;
        //        }

        //        table += "<tr>" +
        //                 "<th></th>"
        //                 +
        //                 "<th></th>"
        //                 +
        //             //"<th>" + db.Procedures.ToList().Find(x => x.ProCode == proCode).ProCost.ToString("R0.00") + "</th>" +
        //             // "<th>" + db.OvernightAdmissions.ToList().Find(x => x.OvernightAdmissionId == AdmCost).calcFinalCost().ToString("") +"</th>"+
        //             "<th>" + ProcedureCost + "" + "</th>" +
        //                 "</tr>";
        //        table += "</table>";


        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return View(vm);

        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(Payment_VM model)
        {
            Payment pay = new Payment();
            Transactions tra = new Transactions();
            //var petv = db.PetVisits.Find(model.VisitId);
            //var pt = petv.PetId;
           // var cu = db.Pets.Find(pt);
            //var cust = cu.OwnerEmail;

            //var mail = db.Customers.Find(cust);
            //db.Payments.Add(new Payment()
            //{
            //    Date = DateTime.Now,
            //    Email = mail.Email,
            //    CustomerId = Convert.ToInt32(cust),
            //    AmountPaid = model.AmountPaid,
            //    PaymentFor = "Transaction " +model.VisitId + " Payment",
            //    PaymentMethod = model.PaymentMethod,
            //});
            db.SaveChanges();
            return RedirectToAction("ProofOfPayment", new { id = model.VisitId });
        }
        
        
        public ActionResult ProofOfPayment(int id)
        {
            //var pv = db.PetVisits.Find(id);
            //ViewBag.PetVisit = pv;
            //// ViewBag. = db.Prescriptions.Find(order.petId);
            //ViewBag.Prescription = db.Prescriptions.ToList().FindAll(x => x.VisitId == pv.VisitId);
            //ViewBag.Total = PrescriptionTotal(id);
            //// ViewBag.Total = db.Prescriptions.ToList().Find(x => x.VisitId == pv.VisitId).PrescriptionTotal();
            //var proc = db.VisitProcedures.ToList().FindAll(x => x.PetVisitID == pv.VisitId);
            //var proCode = pv;
            //ViewBag.VisitProcedure = db.VisitProcedures.ToList().FindAll(x => x.PetVisitID == pv.VisitId);
            //var ProcedureCost = db.VisitProcedures.ToList().Find(x => x.PetVisitID == pv.VisitId);
            //var Procedure_cost = db.Procedures.ToList().Find(x => x.ProCode == ProcedureCost.ProcedureID);
            //ViewBag.Procedure = Procedure_cost;


            //var pt = pv.PetId;
            //var cu = db.Pets.Find(pt);
            //var AdmisCost = db.overnightAdmissions.ToList().Last(x => x.PetId == cu.PetId);
            //var sss = AdmisCost.totalAdmissionCost;
            //ViewBag.AdmisCost = sss;
            // ViewBag.AdmisCost = db.overnightAdmissions.ToList().Find(x => x.OvernightAdmissionId == pv.VisitId).calcFinalCost();
            //var TotalAdmission = db.overnightAdmissions.ToList().Find(x => x.OvernightAdmissionId == AdmCost)
            //ViewBag.Total = TotalAdmission;
            //ViewBag.FinalTotal = Calc(ProcedureCost, TotalAdmission);

            try
            {

                string table = "<br/>" +
                               "Prescription in this order<br/>" +
                               "<table>";
                table += "<tr>"
                         //"<th>MedName</th>"
                         +
                          "<th>isBeforeMeal</th>"
                          +
                           "<th>Duration</th>"
                           +
                         "<th> ExtraNotes</th>"
                         +
                          "<th>Price</th>"
                          +
                         "<th>Qty</th>" +
                         "</tr>";
                //foreach (var dosage in (List<Prescription>)ViewBag.Prescription)
                //{
                //    string dosagesinoder = "<tr> " +
                //                         // "<td>" + dosage.MedName + " </td>" +
                //                         "<td>" + dosage.isBeforeMeal + " </td>" +
                //                         "<td> " + dosage.Duration + " </td>" +
                //                         "<td> " + dosage.ExtraNotes + " </td>" +
                //                         "<td>R " + dosage.Price + " </td>" +
                //                         "<td> " + dosage.Qty + " </td>" +
                //                         "<tr/>";
                //    table += dosagesinoder;
                //}

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                     //"<th>" + db.Procedures.ToList().Find(x => x.ProCode == proCode).ProCost.ToString("R0.00") + "</th>" +
                     // "<th>" + db.OvernightAdmissions.ToList().Find(x => x.OvernightAdmissionId == AdmCost).calcFinalCost().ToString("") +"</th>"+
                    // "<th>" + ProcedureCost + "" + "</th>" +
                         "</tr>";
                table += "</table>";


            }
            catch (Exception ex)
            {

            }



            try
            {
                string table = "<br/>" +
                                               "Procedure in this order<br/>" +
                                               "<table>";
                table += "<tr>"+
                         "<th>VPID</th>"
                          +
                           "<th>DatePeformed</th>"
                           +
                         "<th> Reaction</th>"
                         +
                          "<th>ExtraNotes</th>"
                         +
                         
                         "</tr>";
                //foreach (var dosage in (List<VisitProcedure>)ViewBag.VisitProcedure)
                //{
                //    string dosagesinoder = "<tr> " +
                //                         "<td>" + dosage.VPID + " </td>" +
                //                         "<td> " + dosage.DatePeformed + " </td>" +
                //                         "<td> " + dosage.Reaction + " </td>" +
                //                          "<td> " + dosage.ExtraNotes + " </td>" +
                                         
                //                         "<tr/>";
                //    table += dosagesinoder;
                //}

                //table += "<tr>" +
                //         "<th></th>"
                //         +
                         //"<th></th>"
                       //  +
                     //"<th>" + db.Procedures.ToList().Find(x => x.ProCode == proCode).ProCost.ToString("R0.00") + "</th>" +
                     // "<th>" + db.OvernightAdmissions.ToList().Find(x => x.OvernightAdmissionId == AdmCost).calcFinalCost().ToString("") +"</th>"+
                //     "<th>" + ProcedureCost + "" + "</th>" +
                //         "</tr>";
                //table += "</table>";


            }
            catch (Exception ex)
            {

            }
        
            return View();

        }
        public double PrescriptionTotal(int id)
        {
            double Procedure_cost = 0;
            double amount = 0;
            //foreach (var item in db.Prescriptions.ToList().FindAll(match: x => x.VisitId == id))
            //{
            //    amount += (item.Price * item.Qty) + Procedure_cost;
            //}
            return amount;
        }








        // GET: PaymentEntities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment paymentEntity = db.Payments.Find(id);
            if (paymentEntity == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", paymentEntity.CustomerId);
            return View(paymentEntity);
        }

        // POST: PaymentEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "payment_ID,CustomerId,Email,Date,AmountPaid,PaymentFor,PaymentMethod")] Payment paymentEntity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentEntity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", paymentEntity.CustomerId);
            return View(paymentEntity);
        }

        // GET: PaymentEntities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment paymentEntity = db.Payments.Find(id);
            if (paymentEntity == null)
            {
                return HttpNotFound();
            }
            return View(paymentEntity);
        }

        // POST: PaymentEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment paymentEntity = db.Payments.Find(id);
            db.Payments.Remove(paymentEntity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

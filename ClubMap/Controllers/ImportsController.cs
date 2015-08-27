using System;
using System.Diagnostics;
using System.Web.Mvc;
using ClubMap.Common;
using ClubMap.Logic;
using ClubMap.Models;
using Resources;

namespace ClubMap.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ImportsController : WebController
    {
        // GET: Imports/Import
        public ActionResult Import()
        {
            return View(new ImportViewModel());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ImportFixedClubData(ImportViewModel model)
        {
            var timer = new Stopwatch();
            try
            {
                var path = Server.MapPath(@"~\App_Data\Import.xlsx");
                model.FixedClubDataFile.SaveAs(path);
                timer.Start();
                ImportExcel.ImportFixedData(path);
                System.IO.File.Delete(path);
            }
            catch (Exception e)
            {
                timer.Stop();
                return ImportViewModelViewResult(new Exception(Strings.problem_with_import_excel_file + "\n" + e.Message));
            }
            timer.Stop();
            SetMessageViewModel(Strings.success, MessageViewModelType.success);
            return RedirectToAction("Import");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ImportVariableClubData(ImportViewModel model)
        {
            var timer = new Stopwatch();
            try
            {
                var path = Server.MapPath(@"~\App_Data\Import.xlsx");
                model.VariableClubDataFile.SaveAs(path);
                timer.Start();
                ImportExcel.ImportVariableData(path);
                System.IO.File.Delete(path);
            }
            catch (Exception e)
            {
                timer.Stop();
                return ImportViewModelViewResult(new Exception(Strings.problem_with_import_excel_file + "\n" + e.Message +
                    " (" + (timer.ElapsedMilliseconds / 1000.0) + " seconds)"));
            }
            timer.Stop();
            SetMessageViewModel(Strings.success + " (" + (timer.ElapsedMilliseconds / 1000.0) + " seconds)", MessageViewModelType.success);
            return RedirectToAction("Import");
        }

        private ActionResult ImportViewModelViewResult(Exception exception = null)
        {
            if (exception == null) return RedirectToAction("Import");
            Logger.Log(this, exception);
            SetMessageViewModel(Format(Strings.error_has_occurred, exception.Message), MessageViewModelType.danger);

            return RedirectToAction("Import");
        }
        
    }
}
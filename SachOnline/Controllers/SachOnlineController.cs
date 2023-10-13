using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using PagedList.Mvc;


namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        private string connection;
        private dbSachOnlineDataContext data;
        
        public SachOnlineController()
        {
            // Khởi tạo chuỗi kết nối
            connection = "Data Source=LAPTOP-SD6JFUCG\\MSSQLSERVER01;Initial Catalog=SachOnline;Integrated Security=True";
            data = new dbSachOnlineDataContext(connection);
        }
        public ActionResult NavPartial()
        {
            return PartialView();
        }

        private List<SACH> LaySachMoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        // GET: SachOnline
        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;

            // Retrieve all books from the database, sorted by NgayCapNhat
            var allBooks = data.SACHes.OrderByDescending(a => a.NgayCapNhat);

            // Create a paginated list based on the retrieved and sorted books
            var pagedBooks = allBooks.ToPagedList(pageNumber, pageSize);

            return View(pagedBooks);
        }

        public ActionResult SachBanNhieuPartial()
        {
            // Truy vấn cơ sở dữ liệu để lấy danh sách sách bán nhiều nhất.
            var listSachBanNhieu = data.SACHes.OrderByDescending(a => a.SoLuongBan).Take(6).ToList();

            // Trả về PartialView chứa danh sách sách bán nhiều nhất.
            return PartialView("SachBanNhieuPartial", listSachBanNhieu);
        }

        public ActionResult ChuDePartial()
        {
            var listChuDe = from cd in data.CHUDEs select cd;
            return PartialView(listChuDe);
        }
        public ActionResult NhaXuatBanPartial()
        {
            var listNhaXuatBan = from cd in data.NHAXUATBANs select cd;
            return PartialView(listNhaXuatBan);
        }

        // Lab3
        public ActionResult SachTheoChuDe(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult SachTheoNhaXuatBan(int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach);
        }
        public ActionResult ChiTietSach(int id)
        {
            var sach = from s in data.SACHes 
                       where s.MaSach == id select s;
            return View(sach.Single());
        }
        [HttpGet]
        public ActionResult TimKiem(string tuKhoa)
        {
            ViewBag.tuKhoa = tuKhoa;
            // Thực hiện tìm kiếm sách ở đây và trả về view kết quả
            var sachTimKiem = data.SACHes.Where(s => s.TenSach.Contains(tuKhoa)).ToList();
            return View(sachTimKiem);
        }

        [HttpPost]
        public ActionResult TimKiem(FormCollection form)
        {
            string tuKhoa = form["tuKhoa"];
            ViewBag.TuKhoa = tuKhoa; // Lưu từ khóa để hiển thị trong view
            return RedirectToAction("TimKiem", new { tuKhoa });
        }


    }
}
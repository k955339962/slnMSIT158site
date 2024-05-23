using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using prjMSIT158site.Models;
using prjMSIT158site.Models.DTO;
using System.Linq;

namespace prjMSIT158site.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        // 建構子注入資料庫上下文
        public ApiController(MyDBContext context,IWebHostEnvironment hostEnvironment) 
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            System.Threading.Thread.Sleep(7000);
            // 返回一個簡單的 HTML 內容，使用 UTF-8 編碼
            return Content("Hello,您好", "text/html", System.Text.Encoding.UTF8);
        }


        //讀出不會重複的城市名
        public IActionResult Cities()
        {
            // 從資料庫的 Addresses 表中選取所有唯一的城市名稱
            var cities = _context.Addresses.Select(x => x.City).Distinct();
            // 將結果轉換為 JSON 並返回
            return Json(cities);
        }
        //根據城市名讀出不會重複的鄉鎮區
        public IActionResult Districts(string p)
        {
            var cities2 = _context.Addresses.Where(x => x.City == p);
            var city2 = cities2.Select(x => x.SiteId).Distinct();
            return Json(city2);
        }
        //根據鄉鎮區讀出路名
        public IActionResult Roads(string districts)
        {
            var roads = _context.Addresses.Where(x => x.SiteId == districts).Select(x => x.Road);
            return Json(roads);
        }

        //檢查帳號是否存在
        public IActionResult CheckAccount(string name)
        {
            var member = _context.Members.Any(m => m.Name == name);
            var mail = _context.Members.Any(m => m.Email == name);
            var str = "帳號可使用";
            var str2 = "信箱可使用";
            if (member)
                str = "帳號已存在";
            if(mail)
                str2 = "信箱已存在";
            var str3 = str + "<br />" + str2;
            return Content(str3.ToString(), "text/plain", System.Text.Encoding.UTF8);
        }
        //檢查密碼是否重複&&帳號是否存在
        public IActionResult CheckPassword(Member member, string repassword, IFormFile avatar)
        {
            var is_member = _context.Members.Any(x => x.Name == member.Name);
            var is_mail = _context.Members.Any(x => x.Email == member.Email);
            var is_password = _context.Members.Any(x => x.Password == member.Password);
            var is_repassword = _context.Members.Any(x => x.Password == repassword);
            string str = "密碼相同";
            string str1 = "帳號可使用";
            string str2 = "信箱可使用";
            string str3 = "密碼可使用";

            if(is_member)
                str1 = "帳號已存在";
            if(is_mail)
                str2 = "信箱已存在";
            if(is_password)
                str3 = "密碼已存在";
            if (is_password != is_repassword)
                str = "密碼有誤";
            if (string.IsNullOrEmpty(member.Name))
                member.Name = "guset";

            //實際路徑
            //string upLoadPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", avatar.FileName);
            //string info = upLoadPath;
            //using(var fileStream = new FileStream(upLoadPath,FileMode.Create))
            //{
            //    avatar.CopyTo(fileStream);
            //}

            //檔案上傳轉成二進位
            byte[] imgByte = null;
            using(var memoryStream = new MemoryStream()) 
            {
                avatar.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            //寫進資料庫
            member.FileData = imgByte;
            //_context.Members.Add(member);
            //_context.SaveChanges();

            return Content(str.ToString(), "text/plain", System.Text.Encoding.UTF8);
        }

        public IActionResult Avatar(int id=1)
        {
            // 從資料庫中查找指定 ID 的會員
            Member? member = _context.Members.Find(id);
            // 如果找到會員
            if (member != null)
            {
                byte[] img = member.FileData;
                // 如果會員有頭像數據
                if (img != null) 
                {
                    // 返回圖像文件，並指定 MIME 類型
                    return File(img, "image/jpeg");
                }
            }
            // 如果找不到會員或沒有頭像，返回 404 錯誤
            return NotFound();
        }


        //public IActionResult Register(int id, string name, int age = 20)
        public IActionResult Register(Member member,IFormFile avatar)
        {           
            // 如果name為空或null，將name設置為"guset"
            if (string.IsNullOrEmpty(member.Name))
                member.Name = "guset";


            //取得上傳檔案的資訊
            //string info = $"{avatar.FileName}-{avatar.Length}-{avatar.ContentType}";


            //檔案上傳寫進資料夾
            //todo1 判斷檔案是否存在
            //todo2 限制上傳檔案的大小跟類型 


            //實際路徑
            //string uploadPath = @"C:\Users\User\Documents\workspace\MSIT158Site\wwwroot\uploads\a.png";
            //WebRootPath：C: \Users\User\Documents\workspace\MSIT158Site\wwwroot
            //ContentRootPath：C:\Users\User\Documents\workspace\MSIT158Site

            string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", avatar.FileName);
            string info = uploadPath;

            using(var fileStream = new FileStream(uploadPath,FileMode.Create))
            {
                avatar.CopyTo(fileStream);
            }

            //檔案上傳轉成二進位
            byte[] imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                avatar.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            
            //寫進資料庫
            member.FileName = avatar.FileName;
            member.FileData = imgByte;
            _context.Members.Add(member);
            _context.SaveChanges();

            return Content(info, "text/plain", System.Text.Encoding.UTF8);

            //return Content($"Hello {member.userName}，{member.Age} 歲了，電子郵件是 {member.Email}", "text/html", System.Text.Encoding.UTF8);
        }

        public IActionResult Spots([FromBody] CSearchDTO searchDTO)
        {
            //根據分類編號搜尋景點資料
            var spots = searchDTO.categoryId == 0 ? _context.SpotImagesSpots : _context.SpotImagesSpots.Where(s => s.CategoryId == searchDTO.categoryId);
            //根據關鍵字搜尋景點資料(title、desc)
            if (!string.IsNullOrEmpty(searchDTO.keyword))
                spots = spots.Where(s => s.SpotTitle.Contains(searchDTO.keyword) || s.SpotDescription.Contains(searchDTO.keyword));
            
            //排序
            switch (searchDTO.sortBy)
            {
                case "spotTitle":
                    spots = searchDTO.sortType == "asc" ? spots.OrderBy(s => s.SpotTitle) : spots.OrderByDescending(s => s.SpotTitle);
                    break;
                case "categoryId":
                    spots = searchDTO.sortType == "asc" ? spots.OrderBy(s => s.CategoryId) : spots.OrderByDescending(s => s.CategoryId);
                    break;
                default:
                    spots = searchDTO.sortType == "asc" ? spots.OrderBy(s => s.SpotId) : spots.OrderByDescending(s => s.SpotId);
                    break;
            }

            //總共有多少筆資料
            int totalCount = spots.Count();
            //每頁要顯示幾筆資料
            int pageSize = searchDTO.pageSize;   //searchDTO.pageSize ?? 9;
            //目前第幾頁
            int page = searchDTO.page;

            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            //分頁
            spots = spots.Skip((page - 1) * pageSize).Take(pageSize);


            //包裝要傳給client端的資料
            CSpotsPagingDTO spotsPaging = new CSpotsPagingDTO();
            spotsPaging.TotalCount = totalCount;
            spotsPaging.TotalPages = totalPages;
            spotsPaging.SpotsResult = spots.ToList();

            return Json(spotsPaging);
        }
    }
}

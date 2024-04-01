using AspNetCoreCrudUsingStoredProcedures.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AspNetCoreCrudUsingStoredProcedures.Controllers
{
    public class BooksController : Controller
    {
        private readonly IConfiguration _configuration;

        public BooksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        ////---------------------------------SQL Code To Create Database---------------------------------
        //CREATE TABLE[dbo].[Books]
        //(
        //    [bookId] INT          IDENTITY(1, 1) NOT NULL,
        //    [title]  VARCHAR(100) NOT NULL,
        //    [author] VARCHAR(100) NOT NULL,
        //    [price]  INT NOT NULL,
        //    PRIMARY KEY CLUSTERED([bookId] ASC)
        //);
        ////---------------------------------xxxxxxxxxxxxxxxxxxxxxxxxxxx-------------------------------------




        // GET: Books
        public IActionResult Index()
        {
            List<BookViewModel> books = new List<BookViewModel>();
            //DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("LocalDb")))
            {
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("ViewAllBooks", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    BookViewModel book = new BookViewModel()
                    { 
                        bookId = (int)dataTable.Rows[i]["bookId"],
                        title = (string)dataTable.Rows[i]["title"],
                        author = (string)dataTable.Rows[i]["author"],
                        price = (int)dataTable.Rows[i]["price"]
                    };
                    books.Add(book);
                }
            }
            return View(books);


            ////---------------------------------Stored Procedure For This Method---------------------------------
            //CREATE PROCEDURE[dbo].[ViewAllBooks]

            //AS
            //BEGIN

            //    set nocount on;
            //    select*
            //    from Books
            //END
            //GO
            ////---------------------------------xxxxxxxxxxxxxxxxxxxxxxxxxxx-------------------------------------
        }





        // GET: Books/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = GetBookByID(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }





        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("title,author,price")] BookViewModel book)
        {
            ModelState.Remove(nameof(book.bookId));


            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("LocalDb")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("AddBook", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("title", book.title);
                    sqlCommand.Parameters.AddWithValue("author", book.author);
                    sqlCommand.Parameters.AddWithValue("price", book.price);
                    //sqlCommand.ExecuteNonQuery();
                    await sqlCommand.ExecuteNonQueryAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(book);

            ////---------------------------------Stored Procedure For This Method---------------------------------
            //CREATE PROCEDURE[dbo].[AddBook]
            //    @title varchar(100),
	           // @author varchar(100),
	           // @price int
            //AS
            //BEGIN
            //    set nocount on;
            //    insert into Books(title, author, price)
            //    values(@title, @author, @price)
            //END
            //GO
            ////---------------------------------xxxxxxxxxxxxxxxxxxxxxxxxxxx-------------------------------------
        }









        // GET: Books/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = GetBookByID(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }



        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("bookId,title,author,price")] BookViewModel book)
        {
            if (id != book.bookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("LocalDb")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("EditBookById", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("bookId", book.bookId);
                    sqlCommand.Parameters.AddWithValue("title", book.title);
                    sqlCommand.Parameters.AddWithValue("author", book.author);
                    sqlCommand.Parameters.AddWithValue("price", book.price);
                    sqlCommand.ExecuteNonQuery();
                    //await sqlCommand.ExecuteNonQueryAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(book);

            ////---------------------------------Stored Procedure For This Method---------------------------------
            //CREATE PROCEDURE[dbo].[EditBookById]
            //    @bookId int,
            //    @title varchar(100),
	           // @author varchar(100),
	           // @price int
            //AS
            //BEGIN
            //    set nocount on;
            //    update Books

            //    set
            //        title = @title,
            //        author = @author,
            //        price = @price

            //    where bookId = @bookId
            //END
            //GO
            ////---------------------------------xxxxxxxxxxxxxxxxxxxxxxxxxxx-------------------------------------
        }






        // GET: Books/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = GetBookByID(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("LocalDb")))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("DeleteBookByID", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("bookId", id);
                sqlCommand.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));


            ////---------------------------------Stored Procedure For This Method---------------------------------
            //CREATE PROCEDURE[dbo].[DeleteBookById]

            //    @bookId int

            //AS
            //BEGIN
            //    set nocount on;
            //    delete Books
            //    where bookId = @bookId
            //END
            //GO
            ////---------------------------------xxxxxxxxxxxxxxxxxxxxxxxxxxx-------------------------------------
        }





        [NonAction]
        public BookViewModel GetBookByID(int? id)
        {
            BookViewModel book = new BookViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("LocalDb")))
            {
                DataTable dataTable = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("ViewBookById", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("BookID", id);
                sqlDataAdapter.Fill(dataTable);
                //if (dtbl.Rows.Count == 1)
                //{
                //    bookViewModel.BookID = Convert.ToInt32(dtbl.Rows[0]["BookID"].ToString());
                //    bookViewModel.Title = dtbl.Rows[0]["Title"].ToString();
                //    bookViewModel.Author = dtbl.Rows[0]["Author"].ToString();
                //    bookViewModel.Price = Convert.ToInt32(dtbl.Rows[0]["Price"].ToString());
                //}

                if (dataTable.Rows.Count == 1)
                {
                    book.bookId = (int)dataTable.Rows[0]["bookId"];
                    book.title = (string)dataTable.Rows[0]["title"];
                    book.author = (string)dataTable.Rows[0]["author"];
                    book.price = (int)dataTable.Rows[0]["price"];
                }
                return book;


                ////---------------------------------Stored Procedure For This Method---------------------------------
                //CREATE PROCEDURE[dbo].[ViewBookById]
                //    @bookId int
                //AS
                //BEGIN
                //    set nocount on;
                //    select*
                //    from Books
                //    where bookId = @bookId
                //END
                //GO
                ////---------------------------------xxxxxxxxxxxxxxxxxxxxxxxxxxx-------------------------------------
            }
        }
    }
}

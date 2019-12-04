﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly3.Models; //require models
using Vidly3.ViewModels; //require ViewModels for movies list

namespace Vidly3.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult New()
        {
            var genres = _context.Genres.ToList();

            var viewModel = new MovieFormViewModel
            {
                Genres = genres
            };

            return View("MovieForm", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
                HttpNotFound();

            var viewModel = new MovieFormViewModel
            {
                Movie = movie,
                Genres = _context.Genres.ToList()
            };

            return View("MovieForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Movie movie)
        {
            if (movie.Id == 0) //add
            {
                //set DateAdded to current DateTime
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);
            }
            else //edit
            {
                var movieInDb = _context.Movies.Single(m => m.Id == movie.Id);

                //do not add DateTime since it is not passed in the form
                movieInDb.Name = movie.Name;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.NumberInStock = movie.NumberInStock;
            }
            
            ////from Entity relation error. Fix was to Require GenreId not Genre
            //try
            //{
            //    _context.SaveChanges();
            //}
            //catch (DbEntityValidationException e)
            //{
            //    Console.WriteLine(e);
            //}
            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }

        //GET movies
        public ViewResult Index()
        {
            //var movies = GetMovies();
            var movies = _context.Movies.Include(m => m.Genre).ToList();

            return View(movies);
        }
        //GET Movies/Details/Id
        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();
            

            return View(movie);
        }

        //private IEnumerable<Movie> GetMovies()
        //{
        //    return new List<Movie> {
        //        new Movie { Id=1, Name = "Shrek"},
        //        new Movie { Id=2, Name = "Wall-E"}
        //    };
        //}
        //// GET: Movies/Random
        //public ActionResult Random() //uses Random.cshtml view
        //{
        //    var movie = new Movie() { Name = "Shrek!" };

        //    //add this mock data after creating RandomMovieViewModel
        //    var customers = new List<Customer>
        //    {
        //        new Customer {Name = "Customer 1"},
        //        new Customer {Name = "Customer 2"}
        //    };

        //    var viewModel = new RandomMovieViewModel
        //    {
        //        Movie = movie,
        //        Customers = customers
        //    };

        //    ////ViewData and ViewBag are shit. do not use
        //    ////to use ViewData must reference @ViewData in view
        //    //ViewData["Movie"] = movie; //no longer need to pass data in to View method

        //    //View(model) is short for var viewResult = new ViewResult(); viewResult.ViewData.Model
        //    //movie would be on the Model property like it is in the view

        //    //return View(movie);


        //    //after adding viewModel pass in the entire viewModel to include customers
        //    return View(viewModel);
        //}

        ////Movies/released/{year}/{month}
        ////include contraints with regex and range. 4 digits for year, 2 digits for month
        ////can google ASP.NET Attribute Route Contraints to see others
        //[Route("movies/released/{year:regex(\\d{4})}/{month:regex(\\d{2}):range(1,12)}")]
        //public ActionResult ByReleaseDate(int year, int month)
        //{
        //    return Content(year + "/" + month);
        //}  





        //// GET: Movies/Random
        //public ActionResult Random() //could also return a ViewResult
        //{
        //    var movie = new Movie() { Name = "Shrek!" };

        //    return View(movie); //need view called Random that references movie. View is method from controller class.
        //    //return Content("Hello World!"); //return plain text
        //    //return HttpNotFound(); //return standard 404
        //    //return new EmptyResult(); //return nothing
        //    //return RedirectToAction("Index", "Home", new {page = 1, sortBy = "name"}); //specify action, controller to redirect to, and in object we provide the query string params
        //}

        //public ActionResult Edit(int id) //id parameter taken automatically from url
        //    //do not change name of param because it is {id} in RouteConfig.
        //{
        //    return Content("id=" + id); 
        //}

        //// GET movies. return a query string as text
        //// ? to make param nullible. string is nullable automatically in C#
        //public ActionResult Index(int? pageIndex, string sortBy)
        //{
        //    if (!pageIndex.HasValue) //set default
        //        pageIndex = 1;
        //    if (String.IsNullOrWhiteSpace(sortBy)) //set default
        //        sortBy = "Name";

        //    return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        //}
    }
}
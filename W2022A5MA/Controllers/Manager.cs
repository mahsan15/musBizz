// ************************************************************************************
// WEB524 Project Template V2 2221 == 444f5e98-b836-4276-921c-489d6ce2b21f
// Do not change or remove the line above.
//
// By submitting this assignment you agree to the following statement.
// I declare that this assignment is my own work in accordance with the Seneca Academic
// Policy. No part of this assignment has been copied manually or electronically from
// any other source (including web sites) or distributed to other students.
// ************************************************************************************

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using W2022A5MA.EntityModels;
using W2022A5MA.Models;

namespace W2022A5MA.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper instance
        public IMapper mapper;

        // Request user property...

        // Backing field for the property
        private RequestUser _user;

        // Getter only, no setter
        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        // Default constructor...
        public Manager()
        {
            // If necessary, add constructor code here

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Product, ProductBaseViewModel>();
                cfg.CreateMap<Artist, ArtistBaseViewModel>();
                cfg.CreateMap<Artist, ArtistWithDetailViewModel>();
                cfg.CreateMap<ArtistAddViewModel, Artist>();

                cfg.CreateMap<Album, AlbumBaseViewModel>();
                cfg.CreateMap<Album, AlbumWithDetailViewModel>();
                cfg.CreateMap<AlbumAddViewModel, Album>();
                cfg.CreateMap<Album, AlbumAddViewModel>();

                cfg.CreateMap<Track, TrackBaseViewModel>();
                cfg.CreateMap<TrackAddViewModel, Track>();
                cfg.CreateMap<Track, TrackWithDetailViewModel>();
                cfg.CreateMap<Genre, GenreBaseViewModel>();
                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();
            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }


        // Add your methods below and call them from controllers. Ensure that your methods accept
        // and deliver ONLY view model objects and collections. When working with collections, the
        // return type is almost always IEnumerable<T>.
        //
        // Remember to use the suggested naming convention, for example:
        // ProductGetAll(), ProductGetById(), ProductAdd(), ProductEdit(), and ProductDelete().

        public IEnumerable<GenreBaseViewModel> GenreGetAll()
        {
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(ds.Genres.OrderBy(m => m.Name));
        }

        public IEnumerable<ArtistBaseViewModel> ArtistGetAll()
        {
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(ds.Artists.OrderBy(m => m.Name));
        }
        public ArtistWithDetailViewModel ArtistGetByIdWithDetail(int id)
        {
            var artist = ds.Artists
                .Include("Albums")
                .SingleOrDefault(i => i.Id == id);
            return artist == null ? null : mapper.Map<Artist, ArtistWithDetailViewModel>(artist);
        }

        public ArtistWithDetailViewModel ArtistAdd(ArtistAddViewModel newItem)
        {

           
            // Attempt to add the new item
            var addedItem = ds.Artists.Add(mapper.Map<ArtistAddViewModel, Artist>(newItem));

            // Set the associated item property
          addedItem.Executive = HttpContext.Current.User.Identity.Name;


            ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Artist, ArtistWithDetailViewModel>(addedItem);
          
        }


        
       public AlbumWithDetailViewModel AlbumAdd(AlbumAddViewModel newItem)
        {
            // This method is called from the Vehicles controller...
            // ...AND the Manufacturers controller

            // When adding an object with a required to-one association,
            // MUST fetch the associated object first

            // Attempt to find the associated object
            Artist a = null;
            var l = new List<Artist> { };
            
            foreach (int ArId in newItem.ArtistIds)
            {
                a = ds.Artists.SingleOrDefault(i => i.Id == ArId);
                if (a != null) { l.Add(a); }
                a = null;
            }

            Track t = null;
            var li = new List<Track> { };

            foreach (int trId in newItem.TrackIds)
            {
                t = ds.Tracks.SingleOrDefault(i => i.Id == trId);
                if (t != null) { li.Add(t); }
                t = null;
            }


            if (l.Count == 0 && li.Count == 0)
            {
                return null;
            }
            else
            {
                // Attempt to add the new item
                var addedItem = ds.Albums.Add(mapper.Map<AlbumAddViewModel, Album>(newItem));
                // Set the associated item property
                
                addedItem.Artists = l;
                addedItem.Tracks = li;
                addedItem.Coordinator = HttpContext.Current.User.Identity.Name;
                ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Album, AlbumWithDetailViewModel>(addedItem);
            }
        }


        public IEnumerable<AlbumBaseViewModel> AlbumGetAll()
        {
            return mapper.Map<IEnumerable<Album>, IEnumerable<AlbumBaseViewModel>>(ds.Albums.OrderBy(m => m.Name));
        }
        public AlbumWithDetailViewModel AlbumGetByIdWithDetail(int id)
        {
            var alb = ds.Albums
                .Include("Artists")
                .Include("Tracks")
                .SingleOrDefault(i => i.Id == id);
            return alb == null ? null : mapper.Map<Album, AlbumWithDetailViewModel>(alb);
        }





        public IEnumerable<TrackBaseViewModel> TrackGetAll()
        {
            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(ds.Tracks.OrderBy(m => m.Name));
        }

        public TrackWithDetailViewModel TrackGetByIdWithDetail(int id)
        {
            var track = ds.Tracks
                .Include("Albums.Artists")
                .SingleOrDefault(i => i.Id == id);

            if (track == null)
            {
                return null;
            }
            else
            {
                var result = mapper.Map<Track, TrackWithDetailViewModel>(track);
                result.AlbumNames = track.Albums.Select(a => a.Name);
                return result;
            }
        }
        public TrackWithDetailViewModel TrackAdd(TrackAddViewModel newItem)
        {

            var album = ds.Albums.Find(newItem.AlbumId);


            if (album == null)
            {
                return null;
            }
            else
            {
                // Attempt to add the new item
                var addedItem = ds.Tracks.Add(mapper.Map<TrackAddViewModel, Track>(newItem));
                // Set the associated item property

                addedItem.Albums.Add(album);
                addedItem.Clerk = HttpContext.Current.User.Identity.Name;
         
                ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Track, TrackWithDetailViewModel>(addedItem);
            }
        }

        public IEnumerable<TrackBaseViewModel> TrackGetAllByArtistId(int id)
        {
            var o = ds.Artists.Include("Albums.Tracks").SingleOrDefault(a => a.Id == id);

            if (o == null)
            {
                return null;
            }
            else
            {
                // Attempt to add the new item
                var c = new List<Track>();
                // Set the associated item property

               foreach(var alb in o.Albums) {
                    c.AddRange(alb.Tracks);
                }

                c.Distinct().ToList();
               

                return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(c.OrderBy(t => t.Name));
            }

        }


        //Load Data Methods
        public bool LoadGenreData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // *** Role claims ***
            if (ds.Genres.Count() == 0)
            {
                // Add role claims here

                var pop = new Genre { Name = "Pop" };
                ds.Genres.Add(pop);

                var desi = new Genre { Name = "Desi" };
                ds.Genres.Add(desi);

                var hiphop = new Genre { Name = "Hip-hop" };
                ds.Genres.Add(hiphop);

                var indie = new Genre { Name = "Indie" };
                ds.Genres.Add(indie);

                var rb = new Genre { Name = "R&B" };
                ds.Genres.Add(rb);

                var rock = new Genre { Name = "Rock" };
                ds.Genres.Add(rock);

                var country = new Genre { Name = "Counrty music" };
                ds.Genres.Add(country);

                var kpop = new Genre { Name = "K-pop" };
                ds.Genres.Add(kpop);

                var poprock = new Genre { Name = "Pop rock" };
                ds.Genres.Add(poprock);

                var jazz = new Genre { Name = "Jazz" };
                ds.Genres.Add(jazz);

                ds.SaveChanges();
                done = true;
            }

            return done;
        }


        public bool LoadArtistData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // *** Role claims ***
            if (ds.Artists.Count() == 0)
            {
                // Add role claims here

                var maroon5 = new Artist
                {
                    BirthName = "Adam Levine, Jesse Carmichael, James Valentine, Matt Flynn, PJ Morton and Sam Farrar",
                    BirthOrStartDate = new DateTime(1994, 1, 1),
                    Executive = user,
                    Genre = "Pop",
                    Name = "Maroon 5",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/d/d9/Maroon_5_performing_in_Sydney.jpg"
                }; 
                ds.Artists.Add(maroon5);

                var arijit = new Artist
                {
                    BirthName = "",
                    BirthOrStartDate = new DateTime(1987, 4, 25),
                    Executive = user,
                    Genre = "Desi",
                    Name = "Arijit Singh",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/0/0f/Arijit_5th_GiMA_Awards.jpg"
                };
                ds.Artists.Add(arijit);

                var kanye = new Artist
                { 
                    BirthName = "Kanye Omari West",
                    BirthOrStartDate = new DateTime(1977, 6, 8),
                    Executive = user,
                    Genre = "Rap",
                    Name = "Kanye West",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/1/10/Kanye_West_at_the_2009_Tribeca_Film_Festival_%28cropped%29.jpg"
                };
                ds.Artists.Add(kanye);

               
                var billie = new Artist { 
                    BirthName = "Billie Eilish Pirate Baird O'Connell",
                    BirthOrStartDate = new DateTime(2001, 9, 18),
                    Executive = user,
                    Genre = "Pop",
                    Name = "Billie Eilish",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/f/fa/Billie_Eilish_2019_by_Glenn_Francis_%28cropped%29_2.jpg"

                };
                ds.Artists.Add(billie);

             

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool LoadAlbumData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // *** Role claims ***
            if (ds.Albums.Count() == 0)
            {
                // Add role claims here
                var kanye = ds.Artists.SingleOrDefault(a => a.Name == "Kanye West");
                var alb1 = new Album {
                    Artists = new List<Artist> { kanye },
                    Genre = "Hip-hop",
                    Name = "Graduation",
                    ReleaseDate = new DateTime(2007, 9, 11),
                    Coordinator = user,
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/7/70/Graduation_%28album%29.jpg"
                };
                var alb2 = new Album
                {
                    Artists = new List<Artist> { kanye },
                    Genre = "Hip-hop",
                    Name = "Donda 2",
                    ReleaseDate = new DateTime(2022, 2, 23),
                    Coordinator = user,
                    UrlAlbum = "https://images.genius.com/32e9ee0a056315909f7d6f6a703f2395.1000x1000x1.jpg"
                };
                ds.Albums.Add(alb1);
                ds.Albums.Add(alb2);

                var billie = ds.Artists.SingleOrDefault(a => a.Name == "Billie Eilish");
                var alb_1 = new Album
                {
                    Artists = new List<Artist> { billie },
                    Genre = "Pop",
                    Name = "When We All Fall Asleep, Where Do We Go?",
                    ReleaseDate = new DateTime(2019, 3, 29),
                    Coordinator = user,
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/3/38/When_We_All_Fall_Asleep%2C_Where_Do_We_Go%3F.png"
                };
                var alb_2 = new Album
                {
                    Artists = new List<Artist> { billie },
                    Genre = "Pop",
                    Name = "Happier Than Ever",
                    ReleaseDate = new DateTime(2021, 7, 30),
                    Coordinator = user,
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/4/45/Billie_Eilish_-_Happier_Than_Ever.png"
                };
                ds.Albums.Add(alb_1);
                ds.Albums.Add(alb_2);

                var arijit = ds.Artists.SingleOrDefault(a => a.Name == "Arijit Singh");
                var album1 = new Album
                {
                    Artists = new List<Artist> { arijit },
                    Genre = "Pop",
                    Name = "99 Songs",
                    ReleaseDate = new DateTime(2020, 3, 20),
                    Coordinator = user,
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/0/02/99_Songs_soundtrack.jpg"
                };
                var album2 = new Album
                {
                    Artists = new List<Artist> { arijit },
                    Genre = "Desi",
                    Name = "Dilliwaali Zaalim Girlfriend",
                    ReleaseDate = new DateTime(2015, 3, 9),
                    Coordinator = user,
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/a/ad/Dilliwali_Zaalim_Girlfriend.jpeg"
                };
                ds.Albums.Add(album1);
                ds.Albums.Add(album2);

                var maroon5 = ds.Artists.SingleOrDefault(a => a.Name == "Maroon 5");
                var album_1 = new Album
                {
                    Artists = new List<Artist> { maroon5 },
                    Genre = "Pop",
                    Name = "V",
                    ReleaseDate = new DateTime(2014, 8, 29),
                    Coordinator = user,
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/5/53/Maroon_5_-_V_%28Official_Album_Cover%29.png"
                };
                var album_2 = new Album
                {
                    Artists = new List<Artist> { maroon5 },
                    Genre = "Pop",
                    Name = "Jordi",
                    ReleaseDate = new DateTime(2021, 6, 11),
                    Coordinator = user,
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/d/d7/Maroon_5_-_Jordi.png"
                };
                ds.Albums.Add(album_1);
                ds.Albums.Add(album_2);

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool LoadTracksData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // *** Role claims ***
            if (ds.Tracks.Count() == 0)
            {
                // Add role claims here
                var album = ds.Albums.SingleOrDefault(a => a.Name == "Graduation");
                var track1 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Hip-hop",
                    Name = "Good Morning",
                    Clerk = user,
                    Composers = "Kanye West"
                };
                var track2 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Hip-hop",
                    Name = "Champion",
                    Clerk = user,
                    Composers = "Kanye West"
                };
                var track3 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Hip-hop",
                    Name = "Stronger",
                    Clerk = user,
                    Composers = "Kanye West"
                };
                var track4 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Hip-hop",
                    Name = "Flowers",
                    Clerk = user,
                    Composers = "Kanye West, Chris Martin"
                };
                var track5 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Hip-hop",
                    Name = "Good Life",
                    Clerk = user,
                    Composers = "Kanye West, T-Pain"
                };

                ds.Tracks.Add(track1);
                ds.Tracks.Add(track2);
                ds.Tracks.Add(track3);
                ds.Tracks.Add(track4);
                ds.Tracks.Add(track5);

                 album = ds.Albums.SingleOrDefault(a => a.Name == "Donda 2");
                 track1 = new Track
                 {
                     Albums = new List<Album> { album },
                     Genre = "Hip-hop",
                     Name = "True Love",
                     Clerk = user,
                     Composers = "Kanye West, XXXTENTACION"
                 };
                 track2 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Hip-hop",
                    Name = "Security",
                    Clerk = user,
                    Composers = "Kanye West"
                };
                 track3 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Hip-hop",
                    Name = "Lift Me Up",
                    Clerk = user,
                    Composers = "Kanye West, Vory"
                };
                
                 track4 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Hip-hop",
                    Name = "Too Easy",
                    Clerk = user,
                    Composers = "Kanye West"
                };
                 track5 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Hip-hop",
                    Name = "Pablo",
                    Clerk = user,
                    Composers = "Kanye West, Travis Scott, Future"
                };

                ds.Tracks.Add(track1);
                ds.Tracks.Add(track2);
                ds.Tracks.Add(track3);
                ds.Tracks.Add(track4);
                ds.Tracks.Add(track5);

                album = ds.Albums.SingleOrDefault(a => a.Name == "When We All Fall Asleep, Where Do We Go?");
                track1 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "bad guy",
                    Clerk = user,
                    Composers = "Billie Eilish"
                };
                track2 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "xanny",
                    Clerk = user,
                    Composers = "Billie Eilish"
                };
                track3 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "you should see me in a crown",
                    Clerk = user,
                    Composers = "Billie Eilish"
                };

                track4 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "wish you were gay",
                    Clerk = user,
                    Composers = "Billie Eilish"
                };
                track5 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "bury a friend",
                    Clerk = user,
                    Composers = "Billie Eilish"
                };

                ds.Tracks.Add(track1);
                ds.Tracks.Add(track2);
                ds.Tracks.Add(track3);
                ds.Tracks.Add(track4);
                ds.Tracks.Add(track5);

                album = ds.Albums.SingleOrDefault(a => a.Name == "Happier Than Ever");
                track1 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Getting Older",
                    Clerk = user,
                    Composers = "Billie Eilish"
                };
                track2 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "my future",
                    Clerk = user,
                    Composers = "Billie Eilish"
                };
                track3 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Oxytocin",
                    Clerk = user,
                    Composers = "Billie Eilish"
                };

                track4 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Lost Cause",
                    Clerk = user,
                    Composers = "Billie Eilish"
                };
                track5 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Everybody dies",
                    Clerk = user,
                    Composers = "Billie Eilish"
                };

                ds.Tracks.Add(track1);
                ds.Tracks.Add(track2);
                ds.Tracks.Add(track3);
                ds.Tracks.Add(track4);
                ds.Tracks.Add(track5);

                album = ds.Albums.SingleOrDefault(a => a.Name == "99 Songs");
                track1 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "O Aashiqa",
                    Clerk = user,
                    Composers = "Arijit Singh, A. R. Rahman, Shashwat Singh"
                };
                track2 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Sofia",
                    Clerk = user,
                    Composers = "Arijit Singh, A. R. Rahman, Shashwat Singh"
                };
                track3 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Nayi Nayi",
                    Clerk = user,
                    Composers = "Arijit Singh, Shashwat Singh, Raftaar"
                };

                track4 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Jwalamukhi",
                    Clerk = user,
                    Composers = "Arijit Singh"
                };
                track5 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Teri Nazar",
                    Clerk = user,
                    Composers = "Arijit Singh, Shashwat Singh"
                };

                ds.Tracks.Add(track1);
                ds.Tracks.Add(track2);
                ds.Tracks.Add(track3);
                ds.Tracks.Add(track4);
                ds.Tracks.Add(track5);

                album = ds.Albums.SingleOrDefault(a => a.Name == "Dilliwaali Zaalim Girlfriend");
                track1 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Desi",
                    Name = "Janib",
                    Clerk = user,
                    Composers = "Arijit Singh"
                };
                track2 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Desi",
                    Name = "Tere Liye",
                    Clerk = user,
                    Composers = "Arijit Singh"
                };
                track3 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Desi",
                    Name = "Saddi Dilli",
                    Clerk = user,
                    Composers = "Arijit Singh"
                };

                ds.Tracks.Add(track1);
                ds.Tracks.Add(track2);
                ds.Tracks.Add(track3);
                //Sorry this Album soundtrack only has 3 songs as it is for a movie

                album = ds.Albums.SingleOrDefault(a => a.Name == "V");
                track1 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Maps",
                    Clerk = user,
                    Composers = "Maroon 5"
                };
                track2 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Animals",
                    Clerk = user,
                    Composers = "Maroon 5"
                };
                track3 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Unkiss Me",
                    Clerk = user,
                    Composers = "Maroon 5"
                };

                track4 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "New Love",
                    Clerk = user,
                    Composers = "Maroon 5"
                };
                track5 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Feelings",
                    Clerk = user,
                    Composers = "Maroon 5"
                };

                ds.Tracks.Add(track1);
                ds.Tracks.Add(track2);
                ds.Tracks.Add(track3);
                ds.Tracks.Add(track4);
                ds.Tracks.Add(track5);

                album = ds.Albums.SingleOrDefault(a => a.Name == "Jordi");
                track1 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Lovesick",
                    Clerk = user,
                    Composers = "Maroon 5"
                };
                track2 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Button",
                    Clerk = user,
                    Composers = "Maroon 5"
                };
                track3 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Nobody's Love",
                    Clerk = user,
                    Composers = "Maroon 5"
                };

                track4 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Remedy",
                    Clerk = user,
                    Composers = "Maroon 5"
                };
                track5 = new Track
                {
                    Albums = new List<Album> { album },
                    Genre = "Pop",
                    Name = "Echo",
                    Clerk = user,
                    Composers = "Maroon 5"
                };

                ds.Tracks.Add(track1);
                ds.Tracks.Add(track2);
                ds.Tracks.Add(track3);
                ds.Tracks.Add(track4);
                ds.Tracks.Add(track5);


                ds.SaveChanges();
                done = true;
            }



            return done;
        }


        // *** Add your methods above this line **


        #region Role Claims

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        #endregion

        #region Load Data Methods

        // Add some programmatically-generated objects to the data store
        // You can write one method or many methods but remember to
        // check for existing data first.  You will call this/these method(s)
        // from a controller action.

        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // *** Role claims ***
            if (ds.RoleClaims.Count() == 0)
            {
                // Add role claims here

                var executiveRole = new RoleClaim { Name = "Executive" };
                ds.RoleClaims.Add(executiveRole);

                var CoordinatorRole = new RoleClaim { Name = "Coordinator" };
                ds.RoleClaims.Add(CoordinatorRole);

                var ClerkRole = new RoleClaim { Name = "Clerk" };
                ds.RoleClaims.Add(ClerkRole);

                var StaffRole = new RoleClaim { Name = "Staff" };
                ds.RoleClaims.Add(StaffRole);

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                foreach (var e in ds.Genres)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                foreach (var e in ds.Artists)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                foreach (var e in ds.Albums)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                foreach (var e in ds.Tracks)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }

                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    #endregion

    #region RequestUser Class

    // This "RequestUser" class includes many convenient members that make it
    // easier work with the authenticated user and render user account info.
    // Study the properties and methods, and think about how you could use this class.

    // How to use...
    // In the Manager class, declare a new property named User:
    //    public RequestUser User { get; private set; }

    // Then in the constructor of the Manager class, initialize its value:
    //    User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

    public class RequestUser
    {
        // Constructor, pass in the security principal
        public RequestUser(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                // You can change the string value in your app to match your app domain logic
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            // Compose the nicely-formatted full names
            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }

        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }

        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }

        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }

    #endregion

}
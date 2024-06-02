using FA.LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FA.LibraryManagement.Core.Data;

/// <summary>
///     The application initializer class
/// </summary>
public static class ApplicationInitializer
{
    /// <summary>
    ///     Seeds the model builder
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = 1,
                Name = "Librarian",
                NormalizedName = "LIBRARIAN",
                Description = "This is role : librarian"
            },
            new Role
            {
                Id = 2,
                Name = "Member",
                NormalizedName = "MEMBER",
                Description = "This is role : member"
            }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "trungsangle123@gmail.com",
                FirstName = "Trung",
                LastName = "Sang",
                PasswordHash = "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==",
                UserName = "SangLT12",
                Gender = "Male",
                BirthDate = new DateTime(1999, 12, 3),
                SecurityStamp = "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R",
                ConcurrencyStamp = "225095a0-5ce9-4ebc-a51c-08d11a168f78",
                NormalizedEmail = "TRUNGSANGLE123@GMAIL.COM",
                NormalizedUserName = "SANGLT12",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                PhoneNumber = "0123456789",
                AccessFailedCount = 0,
                LockoutEnd = null
            },
            new User
            {
                Id = 2,
                Email = "hao.nn151201@gmail.com",
                FirstName = "Ngoc",
                LastName = "Hao",
                PasswordHash = "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==",
                UserName = "HaoNN1",
                Gender = "Male",
                BirthDate = new DateTime(2001, 12, 15),
                SecurityStamp = "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R",
                ConcurrencyStamp = "225095a0-5ce9-4ebc-a51c-08d11a168f78",
                NormalizedEmail = "HAONN1@GMAIL.COM",
                NormalizedUserName = "HAONN1",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                PhoneNumber = "0123456789",
                AccessFailedCount = 0,
                LockoutEnd = null
            },
            new User
            {
                Id = 3,
                Email = "vomanhdung276@gmail.com",
                FirstName = "Vo",
                LastName = "Dung",
                PasswordHash = "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==",
                UserName = "DungVM7",
                Gender = "Male",
                BirthDate = new DateTime(2001, 12, 3),
                SecurityStamp = "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R",
                ConcurrencyStamp = "225095a0-5ce9-4ebc-a51c-08d11a168f78",
                NormalizedEmail = "DUNGVM7@GMAIL.COM",
                NormalizedUserName = "DUNGVM7",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                PhoneNumber = "0123456789",
                AccessFailedCount = 0,
                LockoutEnd = null
            },
            new User
            {
                Id = 4,
                Email = "member001@gmail.com",
                FirstName = "Member",
                LastName = "Test 001",
                PasswordHash = "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==",
                UserName = "Member001",
                Gender = "Male",
                BirthDate = new DateTime(2001, 12, 3),
                SecurityStamp = "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R",
                ConcurrencyStamp = "225095a0-5ce9-4ebc-a51c-08d11a168f78",
                NormalizedEmail = "MEMBER001@GMAIL.COM",
                NormalizedUserName = "MEMBER001",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                PhoneNumber = "0123456789",
                AccessFailedCount = 0,
                LockoutEnd = null
            },
            new User
            {
                Id = 5,
                Email = "member002@gmail.com",
                FirstName = "Member",
                LastName = "Test 002",
                PasswordHash = "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==",
                UserName = "Member002",
                Gender = "Male",
                BirthDate = new DateTime(2001, 12, 3),
                SecurityStamp = "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R",
                ConcurrencyStamp = "225095a0-5ce9-4ebc-a51c-08d11a168f78",
                NormalizedEmail = "MEMBER002@GMAIL.COM",
                NormalizedUserName = "MEMBER002",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                PhoneNumber = "0123456789",
                AccessFailedCount = 0,
                LockoutEnd = null
            },
            new User
            {
                Id = 6,
                Email = "member003@gmail.com",
                FirstName = "Member",
                LastName = "Test 003",
                PasswordHash = "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==",
                UserName = "Member003",
                Gender = "Male",
                BirthDate = new DateTime(2001, 12, 3),
                SecurityStamp = "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R",
                ConcurrencyStamp = "225095a0-5ce9-4ebc-a51c-08d11a168f78",
                NormalizedEmail = "MEMBER003@GMAIL.COM",
                NormalizedUserName = "MEMBER003",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                PhoneNumber = "0123456789",
                AccessFailedCount = 0,
                LockoutEnd = null
            },
            new User
            {
                Id = 7,
                Email = "member004@gmail.com",
                FirstName = "Member",
                LastName = "Test 004",
                PasswordHash = "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==",
                UserName = "Member004",
                Gender = "Male",
                BirthDate = new DateTime(2001, 12, 3),
                SecurityStamp = "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R",
                ConcurrencyStamp = "225095a0-5ce9-4ebc-a51c-08d11a168f78",
                NormalizedEmail = "MEMBER004@GMAIL.COM",
                NormalizedUserName = "MEMBER004",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                PhoneNumber = "0123456789",
                AccessFailedCount = 0,
                LockoutEnd = null
            },
            new User
            {
                Id = 8,
                Email = "member005@gmail.com",
                FirstName = "Member",
                LastName = "Test 005",
                PasswordHash = "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==",
                UserName = "Member005",
                Gender = "Male",
                BirthDate = new DateTime(2001, 12, 3),
                SecurityStamp = "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R",
                ConcurrencyStamp = "225095a0-5ce9-4ebc-a51c-08d11a168f78",
                NormalizedEmail = "MEMBER005@GMAIL.COM",
                NormalizedUserName = "MEMBER005",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                PhoneNumber = "0123456789",
                AccessFailedCount = 0,
                LockoutEnd = null
            },
            new User
            {
                Id = 9,
                Email = "member006@gmail.com",
                FirstName = "Member",
                LastName = "Test 006",
                PasswordHash = "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==",
                UserName = "Member006",
                Gender = "Male",
                BirthDate = new DateTime(2001, 12, 3),
                SecurityStamp = "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R",
                ConcurrencyStamp = "225095a0-5ce9-4ebc-a51c-08d11a168f78",
                NormalizedEmail = "MEMBER006@GMAIL.COM",
                NormalizedUserName = "MEMBER006",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                PhoneNumber = "0123456789",
                AccessFailedCount = 0,
                LockoutEnd = null
            },
            new User
            {
                Id = 10,
                Email = "member007@gmail.com",
                FirstName = "Member",
                LastName = "Test",
                PasswordHash = "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==",
                UserName = "Member007",
                Gender = "Male",
                BirthDate = new DateTime(2001, 12, 3),
                SecurityStamp = "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R",
                ConcurrencyStamp = "225095a0-5ce9-4ebc-a51c-08d11a168f78",
                NormalizedEmail = "MEMBER007@GMAIL.COM",
                NormalizedUserName = "MEMBER007",
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                PhoneNumber = "0123456789",
                AccessFailedCount = 0,
                LockoutEnd = null
            }
        );

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                UserId = 1,
                RoleId = 1
            },
            new UserRole
            {
                UserId = 2,
                RoleId = 1
            },
            new UserRole
            {
                UserId = 3,
                RoleId = 1
            },
            new UserRole
            {
                UserId = 4,
                RoleId = 2
            }
        );

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Law"
            },
            new Category
            {
                Id = 2,
                Name = "Teen & Young Adult"
            },
            new Category
            {
                Id = 3,
                Name = "Politics & Social Sciences"
            },
            new Category
            {
                Id = 4,
                Name = "Education & Teaching"
            },
            new Category
            {
                Id = 5,
                Name = "Engineering & Transportation"
            }
        );

        //Add data for book
        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "Reading the Constitution: Why I Chose Pragmatism, Not Textualism",
                CategoryId = 1,
                Description =
                    "A provocative, brilliant analysis by recently retired Supreme Court Justice Stephen Breyer that deconstructs the textualist philosophy of the current Supreme Court’s supermajority and makes the case for a better way to interpret the Constitution.\n\n“You will not read a more important legal work this election year.” —Bob Woodward,Washington Post reporter and author of fifteen #1 New York Times bestselling books\n“A dissent for the ages.” —The Washington Post\n“Breyer’s candor about the state of the court is refreshing and much needed.” —The Boston Globe\n\nThe relatively new judicial philosophy of textualism dominates the Supreme Court. Textualists claim that the right way to interpret the Constitution and statutes is to read the text carefully and examine the language as it was understood at the time the documents were written.\n\nThis, however, is not Justice Breyer’s philosophy nor has it been the traditional way to interpret the Constitution since the time of Chief Justice John Marshall. Justice Breyer recalls Marshall’s exhortation that the Constitution must be a workable set of principles to be interpreted by subsequent generations.\n\nMost important in interpreting law, says Breyer, is to understand the purposes of statutes as well as the consequences of deciding a case one way or another. He illustrates these principles by examining some of the most important cases in the nation’s history, among them the Dobbs and Bruen decisions from 2022 that he argues were wrongly decided and have led to harmful results.\n",
                Quantity = 10,
                ISBN = "978-1668021538",
                Publisher = "Simon & Schuster",
                PublishedDate = new DateOnly(2024, 3, 26)
            },
            new Book
            {
                Id = 2,
                Title = "The Authority of the Court and the Peril of Politics",
                CategoryId = 1,
                Description =
                    "A sitting justice reflects upon the authority of the Supreme Court―how that authority was gained and how measures to restructure the Court could undermine both the Court and the constitutional system of checks and balances that depends on it.\n\nA growing chorus of officials and commentators argues that the Supreme Court has become too political. On this view the confirmation process is just an exercise in partisan agenda-setting, and the jurists are no more than “politicians in robes”―their ostensibly neutral judicial philosophies mere camouflage for conservative or liberal convictions.",
                Quantity = 20,
                ISBN = "978-0674269361",
                Publisher = "Harvard University Press",
                PublishedDate = new DateOnly(2021, 9, 14)
            },
            new Book
            {
                Id = 3,
                Title = "Happy Birthday to You!",
                CategoryId = 2,
                Description =
                    "Say “happy birthday,” Dr. Seuss-style! This classic picture book whisks readers away on the most spectacular birthday of all time—and reminds them to celebrate themselves every day of the year!\n\nI wish we could do what they do in Katroo.\nThey sure know how to say “Happy birthday to you!”\n\nWhen the Great Birthday Bird of Katroo arrives to usher in your “Day of all Days,” you can expect a colorful romp full of fantastical fun that is all about YOU! Treat yourself to flowers that smell like licorice and cheese. Pick out the world’s tallest pet—or a nice Time-Telling Fish. Then prepare for a party so grand it will take twenty days just to sweep up the mess.\n\nFeaturing birthday festivities on every page, this joyful classic from the one and only Dr. Seuss rejoices in the person you were born to be!",
                Quantity = 20,
                ISBN = "978-0394800769",
                Publisher = "Random House Books for Young Readers",
                PublishedDate = new DateOnly(1959, 8, 12)
            },
            new Book
            {
                Id = 4,
                Title = "Oh Say Can You Say? (Dr. Seuss Collector's Edition)",
                CategoryId = 2,
                Description =
                    "Say “happy birthday,” Dr. Seuss-style! This classic picture book whisks readers away on the most spectacular birthday of all time—and reminds them to celebrate themselves every day of the year!\n\nI wish we could do what they do in Katroo.\nThey sure know how to say “Happy birthday to you!”\n\nWhen the Great Birthday Bird of Katroo arrives to usher in your “Day of all Days,” you can expect a colorful romp full of fantastical fun that is all about YOU! Treat yourself to flowers that smell like licorice and cheese. Pick out the world’s tallest pet—or a nice Time-Telling Fish. Then prepare for a party so grand it will take twenty days just to sweep up the mess.\n\nFeaturing birthday festivities on every page, this joyful classic from the one and only Dr. Seuss rejoices in the person you were born to be!",
                Quantity = 20,
                ISBN = "978-0375872334",
                Publisher = "Random House Books for Young Readers",
                PublishedDate = new DateOnly(2013, 11, 5)
            },
            new Book
            {
                Id = 5,
                Title = "The 48 Laws of Power",
                CategoryId = 3,
                Description =
                    "Amoral, cunning, ruthless, and instructive, this multi-million-copy New York Times bestseller is the definitive manual for anyone interested in gaining, observing, or defending against ultimate control – from the author of The Laws of Human Nature.\n\nIn the book that People magazine proclaimed “beguiling” and “fascinating,” Robert Greene and Joost Elffers have distilled three thousand years of the history of power into 48 essential laws by drawing from the philosophies of Machiavelli, Sun Tzu, and Carl Von Clausewitz and also from the lives of figures ranging from Henry Kissinger to P.T. Barnum.\n \nSome laws teach the need for prudence (“Law 1: Never Outshine the Master”), others teach the value of confidence (“Law 28: Enter Action with Boldness”), and many recommend absolute self-preservation (“Law 15: Crush Your Enemy Totally”). Every law, though, has one thing in common: an interest in total domination. In a bold and arresting two-color package, The 48 Laws of Power is ideal whether your aim is conquest, self-defense, or simply to understand the rules of the game.",
                Quantity = 20,
                ISBN = "978-0140280197",
                Publisher = "Penguin Books",
                PublishedDate = new DateOnly(2000, 9, 1)
            },
            new Book
            {
                Id = 6,
                Title = "The Daily Laws and The 33 Strategies of War by Robert Greene 2 Books Collection Set",
                CategoryId = 3,
                Description =
                    "Please Note That The Following Individual Books As Per Original ISBN and Cover Image In this Listing shall be Dispatched The Daily Laws and The 33 Strategies of War by Robert Greene 2 Books Collection Set : The Daily Over the last 25 years, Robert Greene has provided insights into every aspect of being whether that be getting what you want, understanding others' motivations, mastering your impulses, or recognising strengths and weaknesses. The Daily Laws distills that wisdom into easy-to-digest daily entries whose content spans power, seduction, war, strategy, politics, productivity, psychology, leadership, and adversity.Not only is this beautifully designed volume the perfect entry point for those new to Greene's penetrating insight. The 33 Strategies of From bestselling author Robert Greene comes a brilliant distillation of the strategies of war that can help us gain mastery in the modern world. Spanning world civilisations, and synthesising dozens of political, philosophical, and religious texts, The 33 Strategies of War is a comprehensive guide to the subtle social game of everyday life. Based on profound, timeless lessons, it is abundantly illustrated with examples of the genius and folly of everyone from Napoleon to Margaret Thatcher and Hannibal to Ulysses S. Grant, as well as diplomats, captains of industry and Samurai swordsmen.",
                Quantity = 20,
                ISBN = "978-9124292041",
                Publisher = "Profile Books",
                PublishedDate = new DateOnly(2023, 12, 7)
            },
            new Book
            {
                Id = 7,
                Title =
                    "My First Learn-to-Write Workbook: Practice for Kids with Pen Control, Line Tracing, Letters, and More!",
                CategoryId = 4,
                Description =
                    "Help your little one build communication skills with the ultimate writing workbook for kids ages 3 to 5. More than 1 million copies sold!\n\nSet kids up to succeed in school with a learn to write for kids guide that offers letter, shape, and number practice for kindergarten—and beyond. My First Learn-to-Write Workbook introduces early writers to proper pen control, line tracing, and more with dozens of handwriting exercises that engage their minds and boost their reading and writing comprehension.",
                Quantity = 20,
                ISBN = "978-1641526272",
                Publisher = "Rockridge Press",
                PublishedDate = new DateOnly(2019, 8, 27)
            },
            new Book
            {
                Id = 8,
                Title = "The Beginner's Bible: Timeless Children's Stories",
                CategoryId = 4,
                Description =
                    "Millions of children and their parents can’t be wrong. The bright and vibrant illustrations enhance every word of The Beginner’s Bible\u00ae to produce one of the most moving and memorable Bible experiences a young child can have.\n\nThe Beginner’s Bible is where a child’s journey towards a lifelong love of God’s Word begins.\n\nKids will enjoy reading the story of Noah’s Ark as they see Noah helping the elephant onto the big boat. They will learn about the prophet Jonah as they see him praying inside the fish. And they will follow along with the text of Jesus’ ministry as they see a man in need of healing lowered down through the roof of a house.\n\nParents, teachers, pastors, and children will rediscover these beloved parables and so much more as they read more than 90 stories in The Beginner's Bible, just like millions of children before. The Beginner’s Bible\u00ae brand has been trusted for nearly 30 years, with more than 25 million products sold.",
                Quantity = 20,
                ISBN = "978-0310750130",
                Publisher = "Zonderkidz",
                PublishedDate = new DateOnly(2016, 10, 4)
            },
            new Book
            {
                Id = 9,
                Title =
                    "It's Your Ship: Management Techniques from the Best Damn Ship in the Navy, 10th Anniversary Edition",
                CategoryId = 5,
                Description =
                    "The legendary New York Times bestselling tale of top-down change for anyone trying to navigate today's uncertain business seas.\n\nWhen Captain Abrashoff took over as commander of USS Benfold, it was like a business that had all the latest technology but only some of the productivity. Knowing that responsibility for improving performance rested with him, he realized he had to improve his own leadership skills before he could improve his ship. Within months, he created a crew of confident and inspired problem-solvers eager to take the initiative and responsibility for their actions. The slogan on board became \"It's your ship,\" and Benfold was soon recognized far and wide as a model of naval efficiency. How did Abrashoff do it? Against the backdrop of today's United States Navy, Abrashoff shares his secrets of successful management including:\n",
                Quantity = 20,
                ISBN = "978-1455523023",
                Publisher = "Grand Central Publishing",
                PublishedDate = new DateOnly(2012, 10, 9)
            },
            new Book
            {
                Id = 10,
                Title =
                    "100 Cars That Changed the World: The Designs, Engines, and Technologies That Drive Our Imaginations",
                CategoryId = 5,
                Description =
                    "100 Cars That Changed the World showcases vehicles from the end of the nineteenth century to 2020.\nCars are showcased with brief text, color photography, and vintage black and white photography.\nVehicles included:\nThe Ford Model T that put America on wheels.\nThe Volkswagen Beetle that was loved around the world.\nThe Jeep that helped win World War II and popularized off-road adventure.\nThe Pontiac GTO that launched the muscle car era.\nThe Dodge Caravan that changed the way that families travel.\nThe Ford Explorer that ignited the SUV movement.\nThe Tesla Model S that made electric cars exciting.\nAnd many more!\nLarge hardcover book, 144 pages.",
                Quantity = 20,
                ISBN = "978-1645581246",
                Publisher = "Walter Foster Publishing",
                PublishedDate = new DateOnly(2020, 3, 25)
            }
        );

        modelBuilder.Entity<BookImage>().HasData(
            new BookImage
            {
                Id = 1,
                BookId = 1,
                ImageUrl = "https://m.media-amazon.com/images/I/6118UMWll-L._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 2,
                BookId = 1,
                ImageUrl = "https://m.media-amazon.com/images/I/71agAaqCyOL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 3,
                BookId = 2,
                ImageUrl = "https://m.media-amazon.com/images/I/51B5-WqrHVS._SL1000_.jpg"
            },
            new BookImage
            {
                Id = 4,
                BookId = 3,
                ImageUrl = "https://m.media-amazon.com/images/I/61-66t0NIpL._SL1200_.jpg"
            },
            new BookImage
            {
                Id = 5,
                BookId = 4,
                ImageUrl = "https://m.media-amazon.com/images/I/91i1FNtjk4L._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 6,
                BookId = 5,
                ImageUrl = "https://m.media-amazon.com/images/I/611X8GI7hpL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 7,
                BookId = 6,
                ImageUrl = "https://m.media-amazon.com/images/I/81CE5Iz9VwL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 8,
                BookId = 6,
                ImageUrl = "https://m.media-amazon.com/images/I/91-99A2X5dL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 9,
                BookId = 6,
                ImageUrl = "https://m.media-amazon.com/images/I/71SOwtHPbBL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 10,
                BookId = 7,
                ImageUrl = "https://m.media-amazon.com/images/I/71ZIPHcr-PL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 11,
                BookId = 7,
                ImageUrl = "https://m.media-amazon.com/images/I/71P9+nFqnhL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 12,
                BookId = 7,
                ImageUrl = "https://m.media-amazon.com/images/I/71dxLbzeEoL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 13,
                BookId = 7,
                ImageUrl = "https://m.media-amazon.com/images/I/71EYOUrNnXL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 14,
                BookId = 7,
                ImageUrl = "https://m.media-amazon.com/images/I/71ZIPHcr-PL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 15,
                BookId = 7,
                ImageUrl = "https://m.media-amazon.com/images/I/71qx0ZSysML._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 16,
                BookId = 7,
                ImageUrl = "https://m.media-amazon.com/images/I/71d6Ex5i4zL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 17,
                BookId = 7,
                ImageUrl = "https://m.media-amazon.com/images/I/71D8-vq6AYL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 18,
                BookId = 8,
                ImageUrl = "https://m.media-amazon.com/images/I/81Pic6bOvuL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 19,
                BookId = 8,
                ImageUrl = "https://m.media-amazon.com/images/I/91OZJW0dZnL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 20,
                BookId = 9,
                ImageUrl = "https://m.media-amazon.com/images/I/71wWNi5YymL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 21,
                BookId = 9,
                ImageUrl = "https://m.media-amazon.com/images/I/81zNT-wG8hL._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 22,
                BookId = 10,
                ImageUrl = "https://m.media-amazon.com/images/I/71dNRskgj7L._SL1500_.jpg"
            },
            new BookImage
            {
                Id = 23,
                BookId = 10,
                ImageUrl = "https://m.media-amazon.com/images/I/81LDFz47KSL._SL1500_.jpg"
            }
        );

        modelBuilder.Entity<Author>().HasData(
            new Author
            {
                Id = 1,
                Name = "Stephen Breyer"
            },
            new Author
            {
                Id = 2,
                Name = "Dr. Seuss"
            },
            new Author
            {
                Id = 3,
                Name = "Robert Greene"
            },
            new Author
            {
                Id = 4,
                Name = "Crystal Radke"
            },
            new Author
            {
                Id = 5,
                Name = "The Beginner's Bible"
            },
            new Author
            {
                Id = 6,
                Name = "Adam Higginbotham"
            },
            new Author
            {
                Id = 7,
                Name = "Publications International Ltd"
            }
        );

        modelBuilder.Entity<BookAuthor>().HasData(
            new BookAuthor
            {
                BookId = 1,
                AuthorId = 1
            },
            new BookAuthor
            {
                BookId = 2,
                AuthorId = 1
            },
            new BookAuthor
            {
                BookId = 3,
                AuthorId = 2
            },
            new BookAuthor
            {
                BookId = 4,
                AuthorId = 2
            },
            new BookAuthor
            {
                BookId = 5,
                AuthorId = 3
            },
            new BookAuthor
            {
                BookId = 6,
                AuthorId = 3
            },
            new BookAuthor
            {
                BookId = 7,
                AuthorId = 4
            },
            new BookAuthor
            {
                BookId = 8,
                AuthorId = 5
            },
            new BookAuthor
            {
                BookId = 9,
                AuthorId = 6
            },
            new BookAuthor
            {
                BookId = 10,
                AuthorId = 7
            }
        );
    }
}
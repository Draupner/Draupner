Draupner
========

Full stack scaffolding tool for ASP.NET MVC + NHibernate + Castle.Windsor web applications.
Clone or browse the [companion sample](https://github.com/Draupner/DraupnerGeneratedSample) to get an idea of how an scaffolded with Draupner is structured. 


Obtaining Draupner
-------------------
You can get Draunpner either by downloadin a precompiled bundle or building it from source.


### Download Binary
1. Goto the [downloads page](https://github.com/Draupner/Draupner/downloads) and download the latests .zip.
2. Unblock and unzip the file to whereever you want 
3. Setup a "scaffolo_home" environment variable to point to where you unzipped Draupner to
4. Consider adding that same folder to your PATH

and you're ready to go

### Build from Source
1. Clone this repository
2. Install ruby e.g. [rubyinstaller](http://rubyinstaller.org/)
3. Install [RubyGems](http://rubygems.org/)
4. Open a PowerShell console and fire these commands
	> cd Src
	> gem install --remote rake
	> gem install --remote albacore
5. Change to the directory you cloned this repository to and fire this:
	> $Env:scaffold_home=$pwd
6. Now build Draupner and run the tests with these commands:
	> rake test
	> rake package

and you're ready to go

Getting Started
----------------
NB. This getting starting section is very, very rough right now.

This section walks you through creating very simple little application by taking advantage of Draupner.

The below steps assume that Draupner is in your path.

1. Open a PowerShell console and go to where you want to create 
2. To get an overview of what Draupner can do use this command:
	> scaffold.exe
3. To create, setup, build, and run tests for your first Draupner scaffolded project fire these commands:
	> scaffold.exe create-project library
	> cd  library
	> setup.bat
	> rake
	> rake test
4. At this point you'll need to create a be used by the application. Assuming that you have SQL Server Express installed as .\SQLEXPRESS the database can be created by Draupner:
	> scaffolo.exe create-database library
5. Now you have a basic mostly empty Draupner application. Open up the library.sln in Visual Studio and take a look around.
6. Press F5 to run the application. Your browser should shoe a simple hello page.
7. To add the first enity to the application execute this in the console you opened earlier:
	> scaffold.exe create-entity Book
8. Got back to Visual Studio and say yes to reloading. Draupner has created a new file for you: library.Core/Domain/Model/Book.cs, which does not contain much - just an Id field.
9. Edit the `Book` class to: 
	
	public class Book
    {
        public virtual long Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string ISBN { get; set; }
        public virtual string Authors { get; set; }
    }

10. Now run the following command in the console to generate NHibernate mappings and CRUD views for the `Book` enity as registering `Book` and all associated classes with Castle.Windoser and setting up necesarry AutoMapper configuration:
	> scaffold.exe create-crud Book
11. The above command not only scaffold the CRUD operations from UI to DB, but also adds appropriate tests for the generated code. Let's run the new tests:
	> rake test
12. Go back to Visual Studio, reload and review the changes.
13. Press F5 to rerun. Now there is a Book link on the front page, that leads into the CRUD pages for Book. Go on, create a few books.
14. Goto SQL Managment studio inspect library database to get some more feel for what's going on.
15. To make this slightly more interesting let's add a couple of entities more:
	> scaffold.exe create-entity Author
	> scaffold.exe create-entity LibraryCard
16. Edit the new entities and the Book entity so they look as follows:

	public class Book
    {
        public virtual long Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string ISBN { get; set; }
        public virtual IEnumerable<Author> Authors { get; set; }
    }

    public class Author
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
    }

  	public class LibraryCard
    {
        public virtual long Id { get; set; }
        public virtual int Number { get; set; }
        public virtual IEnumerable<Book> Loans { get; set; }
    
        public LibraryCard()
        {
            Loans = new List<Book>();
        }
    }

17. To get CRUD functionality generated for these new entities and for the edited Book entity run the following in the console:
	> scaffold.exe create-crud Book
	> scaffold.exe create-crud Author
	> scaffold.exe create-crud LibraryCard
18. NB. Due to a limitation of Draupner we need to change the mapping code for the two IEnumerables in the enitites above from using a References mapping to a using a HasMany mapping. To do so edit the mapping classes in the ´library.Core.Common.NHibernate´ namespace to match the following:

 	public class BookMap : ClassMap<Book>
    {
        public BookMap()
        {
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.ISBN);
            HasMany<Author>(x => x.Authors);
        }
    }
	
    public class AuthorMap : ClassMap<Author>
    {
        public AuthorMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }

    public class LibraryCardMap : ClassMap<LibraryCard>
    {
        public LibraryCardMap()
        {
            Id(x => x.Id);
            Map(x => x.Number);
            HasMany(x => x.Loans);
        }
    }

19. To ensure the above went well, rerun the tests (and notice the number of tests added by Draupner):
	> rake test
20. Now the only thing that remains is to F5 and play around

These steps should result in an application similar to the [Draupner companion sample](https://github.com/Draupner/DraupnerGeneratedSample).

Contributing
-------------
We welcome pull reqs!
To make it easy for us to review and (hopefully) accept your pull request please follow the worflow described [here](https://github.com/NancyFx/Nancy/wiki/Git-Workflow) - except refer to Draupner instead of Nancy.

Copyright
---------
Copyright © 2012 Mjølner Informatics A/S ([www.mjolner.dk](http://www.mjolner.dk))

Licensing
----------
Draupner is released under the [Apache 2.0 license](http://www.apache.org/licenses/LICENSE-2.0.html).
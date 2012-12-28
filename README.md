Draupner
========

Full stack scaffolding tool for ASP.NET MVC web applications. Applications built with Draupner use the following list of libraries to create a full stack web app from database to UI:

 * ASP.NET MVC
 * NHibernate
 * FluentNHibernate
 * Castle.Windsor
 * Log4Net
 * AutoMapper
 * Elmah

And on the testing side Draupner applications use:
  
  * xUnit.NET
  * RhinoMocks
  * AutoFixture
  * SQLite

Clone or browse the [companion sample](https://github.com/Draupner/DraupnerGeneratedSample) to get an idea of how a project scaffolded with Draupner is structured. 


Obtaining Draupner
-------------------
You can get Draupner either by downloading a precompiled bundle or building it from source.


### Download Binary
1. Goto the [downloads page](https://github.com/Draupner/Draupner/downloads) and download the latest .zip.
2. Unblock and unzip the file to wherever you want 
3. Setup a "scaffold_home" environment variable to point to where you unzipped Draupner to
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
NB. This getting starting section is still somewhat rough.

This section walks you through creating a very simple little application by taking advantage of Draupner.

The below steps assume that Draupner is in your path.

1. Open a PowerShell console and go to where you want to create 
2. To get an overview of what Draupner can do use this command:

	> scaffold.exe
	
3. To create, setup, build, and run tests for your first Draupner scaffolded project fire these commands (skipping the setup.bat if you already have ruby set up):

	> scaffold.exe create-project Library
	
	> cd  Library
	
	> setup.bat
	
	> rake
	
	> rake test
	
4. At this point you'll need to create a database to be used by the application. Assuming that you have SQL Server Express installed as .\SQLEXPRESS the database can be created by Draupner:

	> scaffold.exe create-database Library
	
5. Now you have a basic mostly empty Draupner application. Open up the Library.sln in Visual Studio and take a look around.
6. Press F5 to run the application. Your browser should show a simple hello page. -If you have trouble here, it may help to go into properties on the Library.Web project and switch from IIS to Visual Studio Development Server.
7. To add the first enity to the application execute this in the console you opened earlier:

	> scaffold.exe create-entity Book
	
8. Go back to Visual Studio and say yes to reloading. Draupner has created a new file for you: library.Core/Domain/Model/Book.cs, which does not contain much - just an Id field.
9. Edit the `Book` class to: 

        public class Book
         {
        	public virtual long Id { get; set; }
        	public virtual string Title { get; set; }
        	public virtual string ISBN { get; set; }
        	public virtual string Authors { get; set; }
    	}

10. Now run the following command in the console to generate NHibernate mappings and CRUD views for the `Book` entity as registering `Book` and all associated classes with Castle.Windser and setting up necesarry AutoMapper configuration:

	> scaffold.exe create-crud Book
	
11. The above command not only scaffolds the CRUD operations from UI to DB, but also adds appropriate tests for the generated code. Let's run the new tests:

	> rake test
	
12. Go back to Visual Studio, reload and review the changes.
13. Press F5 to rerun. Now there is a Book link on the front page, that leads into the CRUD pages for Book. Go on, create a few books.
14. Goto SQL Managment Studio inspect library database to get some more feel for what's going on.
15. To make this slightly more interesting let's add a couple more entities:

	> scaffold.exe create-entity Author
	
	> scaffold.exe create-entity LibraryCard
	
16. Edit the new entities and the Book entity so they look as follows:

        public class Book
    	{
        	public virtual long Id { get; set; }
        	public virtual string Title { get; set; }
        	public virtual string ISBN { get; set; }
        	public virtual ICollection<Author> Authors { get; set; }
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
    		public virtual ICollection<Book> Loans { get; set; }
    	}

17. To get CRUD functionality generated for these new entities and for the edited Book entity run the following in the console:
	
	> scaffold.exe create-crud Book

	> scaffold.exe create-crud Author
	
	> scaffold.exe create-crud LibraryCard

18. To ensure the above went well, rerun the tests (and notice the number of tests added by Draupner):
	
	> rake test

19. Now the only thing that remains is to F5 and play around

These steps should result in an application similar to the [Draupner companion sample](https://github.com/Draupner/DraupnerGeneratedSample).

Contributing
-------------
We welcome pull reqs!
To make it easy for us to review and (hopefully) accept your pull request please follow the workflow described [here](https://github.com/NancyFx/Nancy/wiki/Git-Workflow) - except refer to Draupner instead of Nancy.

Copyright
---------
Copyright © 2012 Mjølner Informatics A/S ([www.mjolner.dk](http://www.mjolner.dk))

Licensing
----------
Draupner is released under the [Apache 2.0 license](http://www.apache.org/licenses/LICENSE-2.0.html).
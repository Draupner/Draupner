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

scaffold.exe create-project library
cd  library
setup.bat

rake
rake test

scaffolo.exe create-database library

F5 -> simple hello world page

scaffold.exe create-entity Book
Got to VS and say yes to reload, and open library.Core/Domain/Model/Book.cs and add  string Title, string Authors and string ISBN virtual auto props

scaffold.exe create-crud Book
rake test <- tests have been added
Got VS

F5 -> app with CRUD pages for books
Create a few
Goto SQL Managment studio inspect library db

scaffold.exe create-entity Author
scaffold.exe create-entity LibraryCard

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

scaffold.exe create-crud Book
scaffold.exe create-crud Author
scaffold.exe create-crud LibraryCard

Change mapping for the IEnumerables from References to HasMany in FluentNH

rake test

F5

Play around :-)

resulting in an app similar to the one in the Draupner sample repo.

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
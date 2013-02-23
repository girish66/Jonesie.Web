Jonesie.Web
===========
A modern, modular ASP.Net MVC 4 web site application.

This is a home brew framework that can be used as a starting point for bespoke projects.  
I've made this open source so that others can share in this work and also so that it can benefit from the input from the community.
Currently the framework works, but could use some reviewing and polishing in a few areas.


Features
--------

* Easy extensibility - create modules in a few minutes, very little overhead to learn.
* Automatic loading of modules - simply drop the DLL in the bin folder and update the wb.config if required.

Tech
----

* ASP.Net MVC 4 (.Net 4 or 4.5.).
* Services services for logging, caching, data access etc.
* Clean HTML 5 stlye and functionality via Twitter Bootstrap, Backbone, Underscore, Handlebars etc.
* Dependency Injection using MEF 2.
* SignalR for real-time client/server communication.
* Dapper for database access (currently MS SQL but could easily target other platforms).

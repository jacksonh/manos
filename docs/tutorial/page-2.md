Getting Started with Manos
==========================

This is page two of the Getting Started with Manos tutorial.  It assumes you already have
Manos installed and have created the Hello World App from page one.


Templates Overview
------------------

Normally we want to spit out something other than just plain strings from our actions. Usually in a web
application we want to create HTML or maybe JSON.  Instead of forcing you to create these formats from hand,
Manos includes a powerful templating engine that can convert a .NET object into a document.

![Diagram showing an object being converted to a document](http://github.com/jacksonh/manos/raw/master/docs/tutorial/manos-template-engine-flow.png)


Creating the Hello World Template
---------------------------------

To create our Hello World template we need to create an index.html page in our Templates/ directory and add the following text:

    <html>
      <head>
        <title>Hello World!</title>
      </head>
      <body>
         Hello World!
      </body>
    </html>


Rendering our Hello World Template
----------------------------------

Because Manos's template engine turns templates into CLR objects, we can now call our templates render method from our action.  Lets
update the action code to look like this:

    Get ("/", ctx => ctx.Render ("index.html"));


Sending Data to our Templates
-----------------------------

In the last two examples you might have wondered what the null parameter is for.  This parameter is how we pass data into the
template engine.  When templates are rendered, the template engine will try to resolve properties on the supplied data.  So if we want
to pass a name into our template, all we need to do is this:

    Get ("/", ctx => ctx.Render ("index.html", new { Name = "Manos" }));

and update our template to use that name:

    <html>
      <head>
        <title>Hello World!</title>
      </head>
      <body>
         Hello {{ Name }}!
      </body>
    </html>

The {{ }} brackets tell the template engine to render the supplied expression to a string. In this case our expression is a variable
named Name, that happens to be a string property on the supplied object.  Variables can also be defined in the template or be part of
an expression, like a for loop:

defined variable:
    ....
      {% set LastName = "World" %}
      <body>
        Hello {{ Name }} {{ LastName }}!
      </body
    ....

for loop:
    ....
      <body>
        <ul>
          {% for item in collection %} 
            <li>{{ item }}</li>  
          {% endfor %}
        </ul>
      </body>
    ....


Building and Running Templates
------------------------------

To build and run our templates, we'll use the manos command again.

    manos -build
    manos -run

if we navigate to http://localhost:8080/ we get a real web page.  Viewing the source should look something like this:

    <html>
      <head>
        <title>Hello World!</title>
      </head>
      <body>
         Hello Manos!
      </body>
    </html>

Template Inheritance
--------------------

Most sites will look almost exactly the same on every single page. Rather than update every page every time we change
the site's layout, it would be nice to share that structure between all of our pages.  Manos makes this easy with
template inheritance. Template inheritance allows you to create a basic page that will be shared between a number of
pages and add blocks to your base page that can be set by each individual page.  Here is our Hello World example rewritten
to use a base page.

Templates/base.html:

    <html>
      <head>
        <title>{% block title %}Hello{% endblock %}</title>
      </head>
      <body>
        {% block content %}
          This page has no content.
        {% endblock %}
      </body>
    </html>

Templates/index.html:

    {% extends "base.html" %}

    Since we are using an extends statement,
    everything outside of code statements will
    be discarded.

    {% block title %}Hello World!{% endblock %}

    {% block content %}
      Hello {{ Name }}!
    {% endblock %}



Template Operations
------------------

Here are some of the cool things you can do with Manos's template engine. For a more in depth
look at the templating engine, checkout the Templates Guide.

### Filters
Filters allow you to easily convert text from one format to another. For example, using the uppercase filter:

    {{ "hELlo" | uppercase }}

will create this:

    HELLO

There are a number of useful filters for rendering markdown, extracting parts of a date or time and parsing URLs. All
of the available filters are listed in the Templates Guide. If the supplied filters don't meet your needs, you can
always add your own filters.

### Conditional statements
Manos supports if, elif and else statements.

    {% if show_name %}
      {{ Name }}
    {% else %}
      Sorry we can't show you the name.
    {% endif %}


### Macros
Macros are a lot like methods in C#.  They allow you to encapsulate and reuse code easily.  Macros can accept parameters
and support default values for parameters.

    {% macro print_button (name, style='big') %}
      <input type="button" name="{{ name }}" style="{{ style }}"></input>
    {% endmacro %}

    ....

    {% print_button ('small-button', 'small') %}
    {% print_button ('big-button') %}

### Includes
Includes allow you to include another file into your template.  Includes can be evaluated at build time or at runtime,
depending on the supplied value.

This will be evaluated at build time:

    {% include "some-file.html" %}
    
And this will be evaluated at run time:

    {% include some_variable %}

### Imports
If you have a bunch of macros that you would like to share between templates you can put them in a common file and
use the import statement:

    {% include "macros.html" as macros %}

    ....
    {% macros.print_calendar () %}


Moving On
---------

Now that we have a good idea of how templates work, we'll go over adding unit tests to your code in part three.


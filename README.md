# michaelkjellander.se - Blazor version

Upcoming replacement for my michaelkjellander.se website. WORK IN PROGRESS.

The site is built using the full-stack framework Blazor. API endpoints aren't necessary, 
but they are used for the following reasons:
1. It makes it easy to swap frontend (or backend), or build a completely separate app, e.g.
a mobile app. I've tried out this idea by developing a separate front using NextJS 
([MichaelKjellander-frontend](https://github.com/Apmannen/MichaelKjellander-frontend)), but
it's not prioritized.
2. It makes it super easy to cache the data request responses.
3. To give it more of a typical .NET backend, with some tweaks according to my own taste. 
Blazor is pretty niche.

The site looks almost identical to my previous Wordpress blog. That's because I fetch
all previous posts from that site's API and since I've grown quite fond of the existing design
I want to keep it. After all, I'm not a graphical designer, I'm a full-stack developer and
enjoy recreating a design using a different framework.

External plugins are intentionally kept at a minimum since in the long run you always run
into problems with them, and the point of this challenge is for me to dive deeper into
.NET and to learn Blazor. Only Interactive Server Side Rendering (SSR) is used at the moment,
so some JavaScript for some client functionality is unavoidable. Stuff like tracking user 
scrolling is as far as I know not possible otherwise.

Tailwind CSS is used for styling. It's pretty lightweight and easy to use. However, I often
write custom CSS, but since I usually keep that isolated in the component CSS files (.razor.css) 
it's not a problem. What I remember from the old days is that you want to avoid filling up the 
global CSS files, it gets very messy very quickly. Either way, I always use Tailwind for 
margins/paddings, which is possible even in CSS files using the @apply directive.

## Launch dev environment

**Compile CSS files**
```
npm run build-styles
```

**Migrate database (environment variables depend on local MySQL setup)**
```
dotnet ef migrations add BlogMigration --context BlogDataContext
dotnet ef migrations add WebGamesMigration --context WebGamesDataContext
$Env:SG_MYSQLCONNSTRING="Server=127.0.0.1;Database=kjelle_db;User=root;Port=3306;SslMode=none;Password=test;" ; dotnet ef database update --verbose --context BlogDataContext
$Env:SG_MYSQLCONNSTRING="Server=127.0.0.1;Database=kjelle_db;User=root;Port=3306;SslMode=none;Password=test;" ; dotnet ef database update --verbose --context WebGamesDataContext
```

**Fetch data from Wordpress API and update local database**
```
dotnet run --launch-profile "clean-wp-db"
```
**Run with hot/live reloading**
```
dotnet run --launch-profile "home"
```
Dev environment profiles are set in Properties/launchSettings.json

-----

Once again, the project is under construction and some temporary solutions will be replaced
with better ones once the project is finished.

<h1>Movie Web Application</h1> 

<h2>Versions - Github Repository</h2>
<ul>
  <li><b>master</b>      - Original version using MVC and Deployed to https://movies.pgmp.se</li>
  <li><b>API-Version</b> - Added API, Scalar</li>
</ul>

<h2>Introduction </h2>
  <p>Movie listing application that supports creating, editing, searching, and listing movies from a database.</p>
  <p>The application lets you add, edit, and delete movies, playlists and Users , as well as see details about individual ones.</p>  
  <ul>
    <li>Creating an MVC Project and Adding MVC Controllers</li>
    <li>Adding Views Create Database</li>
    <li>Creating a Connection String and Working with SQL Server Local DB</li>
  </ul>
  
  <p>A search bar functionality is also used to Find a particular Title of the movie.</p> 
    
<h2>Movies</h2>
  <p>Create, Read, Update and Delete</p> 
  <p>Add Movie to a Playlist</p>

<h2>Users Playlists</h2>
  <p>Create, Read, Update and Delete their playlists</p>
  <p>View Movies in playlist, delete Movies from the playlist</p>

<h2>User Profile</h2>
  <p>Change information for location using IP Address for the user</p>
  <p>Change profile picture</p>
  <p>Create, Read and Update Movies.</p>
  <p>Create, Read, Update and Delete Playlists</p>

<h2>Users</h2>
  <p>Create, Read, Update and Delete(Admin role only) Users</p>

<h2>Users Authorization</h2>
  <p>For Authentication, user must register first and then Log-in to get the full access of the website.</p> 
  <p>Authentication is required for securely validating the user identity and it is a precursor to authorization.</p>  
  <p>Authorization policies start after the authentication process completes.</p> 
  <p>The authorization process determines what data the user can access.</p>
  <p>Access is applied through the main web menu options</p>
  <p>The authorization process determines what data the user can access.</p>
  <ul>
    <li><h3>Admin</h3> - Have access to all CRUD Operations and Database Access</li>
    <li><h3>Authenticated Users</h3> - Can create, read and update their movies and Playlists and update their profiles information</li>
    <li><h3>Non users</h3> - Can only see movies and users in the application</li> 
  </ul>

<h2>Project Template Backend</h2>
  <p>Authentication and Authorization</p> 
  <p>Database (Microsoft SQL Server) the creator should give role to admin.</p> 
  <p>System and Frontend</p> 
  
<h2> Getting started</h2>
<p>Create and set up the database using \Data\Migrations.</p>
<p>Add connection string to app settings.json. It will look something like this:</br><strong>
  Data Source=DESKTOP\\SQLEXPRESS;Initial Catalog=MovieDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False</strong></p>
<p>Register for a Cloudinary Account and add Cloudname, ApiKey, and Api secret to appsettings.json. </p>
<p>Register for a IPInfo Account and add your personal Token in <strong>HomeController.cs</strong> at <strong>Task Index()</strong></p>


<h2>Tech used in this project</h2> 
  <ul>
    <li>Backend: C#, ADO.NET</li>
    <li>DataBase: Microsoft SQL Server Entity framework</li>
    <li>Front-end: ASP.Net, Razor Pages, CSS, Bootstrap.</li> 
    <li>Extras:
      <p>Photo Service - <a href="https://cloudinary.com/">Cloudinary.com</a> </p>
      <p>IP Location service - <a href="https://ipinfo.io">IPInfo.io<a> </p>
      <p>Anonimous photos - <a href="http://unsplash.com">Unsplash.com</a></p> 
   </li>
  </ul>

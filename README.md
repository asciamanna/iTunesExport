iTunesExport
===================
A Console App that parses an iTunes Library file and exports album data to a SQL CE database.
It writes iTunes album information (merged with LastFM play counts and album art location)
to a single SQL CE table.
It relies on the iTunesLibraryParser and LastFMClient projects also on github.
The location of the iTunes Library file, LastFM user, and LastFM api key 
are specified in the exe's app.config.

Anthony Sciamanna
asciamanna@gmail.com
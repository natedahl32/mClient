# mClient
World of Warcraft client library written in C#

The goal of this project is to create a headless WoW client for bots for use on emulation servers. The end goal is a fully autonomous bot that can act and behave as a real player would. This is a definitely a lofty goal and will take years to come to fruition. The project will be dealt with in milestones that will allow the project to move closer to the end goal.

First, let us give credit where credit is do. This project would not be possible without the contributions 
and work of the following people/teams.

justMaku - For creating the original version of mClient that can connect to an emulated WOTLK server.
cMangos Team - For creating the core that I have found a plethora information from and learned a great deal from
WCell Team - For converting a lot of C++ code that I had no idea what it was doing to C#
All other server core teams - For continually pushing forward the emulation boundaries
USB, ClassicDB and other content teams - For all your hard work in populating the worlds with database content

## Benefits of M Client compared to other Bots

There biggest benefit that can be realized by this project is a core agnostic Bot. All other Bots are built directly into the core itself, so they don't work for other cores without being ported. Since this Bot project is client based, it should work with any core. No more having to choose which core you are going to use based on the Bot functionality available!

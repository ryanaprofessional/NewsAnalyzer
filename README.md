### Summary 
  Queries NewsApi.org and then feeds that data to OpenAi and receives a summary.

### Setup
  1. Get an api key from NewsApi.org
  2. Get an api key from OpenAi
  3. Paste this into your user secrets: </br>
   { </br>
     "NewsApiKey": "<$Paste Api Key Here>", </br>
     "JsonFilePath": "<$Paste a directory on your computer here, don't forget to double backslash>", </br>
     "OpenAiKey": "<$Paste Api Key Here>" </br>
   } </br>


### WIP
  This is still a work in progress, future PRs will allow for:
   more advanced news queries and ai analyzation capabilities
   utilization of cloud services
   retrieving data from other 3rd party apis besides news api
   and eventually a full-fledged front-end.  

### Structure: 
  This repository is currently built as a monolith with a bit of duplicative code in order to allow it to easily be broken up into microservices if necessary.  

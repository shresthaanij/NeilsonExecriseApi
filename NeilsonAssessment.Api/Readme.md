# How to Start

 - Pull the lastest master from GitHub, 
 git clone https://github.com/shresthaanij/NeilsonExecriseApi.git
 - If you have docker, from inside folder,
    docker image -t builld myimageapi .
    docker container run -itd --name neilsonapi -p 80:80 neilsonapi
    (now from Postman, nagivate to url, http://localhost/api/cars)
 - If debug from Visual Studio, run the solution.

Basic Authentication
- While hitting the endpoint, add [Authorization] header with (Basic username:password -> in base64). username and password must be same example, api:api


### Workings
- From first run, it ll automatically add 5 pets and 5 cars to memory.
- From there, you can perform CRUD operation in pets and cars endpoints as well as log/schedule gromming dates and veterinarian visist for pets and log/schedule car washes adn visits to mechanic for cars.ndpoint, add [Authorization] header with (Basic username:password -> in base64). username and password must be same example, api:api
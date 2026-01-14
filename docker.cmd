docker build --progress=plain -t my-web-app -f ./WebAppMVC/Dockerfile .

docker run -d   --name my-webapp-mvc   -p 5050:5050 -p 5055:5055      my-web-app


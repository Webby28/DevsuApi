1 - Se adjunta sql con query para creación de tablas. Mirar "BaseDatos.sql"
2 - Se adjunta sql con query para crear usuario necesario que utiliza la API. Mirar "BaseDatos.sql"
3 - Se utilizó Oracle 21c Express Edition.
4 - La cadena de conexión se encuentra en la propiedad "Oracle" de "ConnectionStrings" en appsettings.json en WebApi.
5 - Repositorio GitHub: https://github.com/Webby28/DevsuApi
6 - Se adjunta colecciones de postman utilizadas: Movimientos.postman_collection y Persona.postman_collection
Scripts dockers usados:
docker build -t devsuapi .
docker run -d -p 8080:8080 --name webapp devsuapi
docker run -d --name seq-dev --restart unless-stopped -p 5431:80 -v "$(pwd)/seq-dev:/data" -e ACCEPT_EULA=Y datalust/seq
# SQLServerCSharp
Forma en que utilizo C# para trabajar con bases de datos SQL Server

- Clase SQLServer: en la que realizo conexión a la base de datos y la mantengo para usarla en todo el proyecto.
- La cadena de conexión a la base de datos la defino en la configuración del proyecto, así está disponible en un solo lugar para ser utilizada en cualquier formulario o clase.

- Creo una clase para cada tabla de la base de datos, en la que defino todos los métodos para listar, crear, actualizar y eliminar registros.

El código tiene comentarios para describir los bloques que realizan una tarea específica. Trato de no realizar código difícil de leer para que sea totalmente intuitivo y no dependa de grandes bloques de comentarios.

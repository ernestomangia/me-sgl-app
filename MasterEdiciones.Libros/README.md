# Master Ediciones - Sistema Gestion de Libros
Proyecto final para la asignatura Taller de integración de la carrera Licenciatura en Sistemas de Información.

Brinda soporte para administrar Clientes, Cobradores, Vendedores, Proveedores, Productos, Planes de pago, Gastos, Ventas, Compras entre otras.
Ademas cuenta con un modulo para la generación de reportes.

## Stack
La app se diseño en base a una arquitectura de 4 capas. A continuación se detalla brevemente cada una de ellas:

1. Presentación. Es la capa de fron-end. Se utiliza Bootstrap 3. 
2. Negocio. Se compone de servicios que persisten las entidades del dominio.
3. Acceso a Datos. Entidades del dominio y configuraciones de cada una de ellas.
4. Transversal. Contiene funcionalidades comunes a todas las capas, como Log, y definiciones de enums.

## Setup
1. Descargar la solución completa.
2. Abrir la solucion con Visual Studio 2013.
3. Configurar la conexión a la BD (provado con SQL server) en el web.config del proyecto ME.Libros.Web.
4. Generar un build de la solucion llamada ME.Libros.
5. Ejecutar el proyecto ME.Libros.Web.

## Deployment
El instalador de la aplicación web se genera utilizando InstallShield Limited Edition for VS. 

Por lo que, es necesario descargar el complemento InstallShield registrandose en el siguiente sitio: http://learn.flexerasoftware.com/content/IS-EVAL-InstallShield-Limited-Edition-Visual-Studio 

### Autores
Ernesto Mangia; Denis Omar; Francisco Gregorutti; Dante Tortul Cuatrin


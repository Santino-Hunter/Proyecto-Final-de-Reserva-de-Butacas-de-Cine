# Reserva de Butacas de Cine
Este es un sistema de reserva de butacas para cines, desarrollado en C# utilizando Windows Forms. El proyecto permite gestionar la reserva de asientos en una sala de cine, mostrar estadísticas de ventas y manejar la información de los clientes.

## Funcionalidades
**Reserva de Butacas**: Permite a los usuarios reservar asientos en una sala de cine.

**Confirmación de Reserva**: Los usuarios pueden confirmar sus reservas, lo que actualiza la disponibilidad de los asientos.

**Visualización de Sala**: Muestra la disposición de las butacas y su estado (reservada o libre).

**Estadísticas de Venta**: Genera un informe sobre las butacas reservadas y la facturación generada.

## Estructura del Proyecto
El proyecto está compuesto por las siguientes clases:

**ClienteLSE**: Representa un cliente en la lista simple enlazada, con propiedades para el nombre del cliente, las butacas reservadas y el total a pagar.

**Sala**: Gestiona una lista de clientes y operaciones relacionadas como agregar y eliminar clientes, y buscar butacas reservadas.

**FRMReserva**: Interfaz para gestionar las reservas de las butacas. Permite a los usuarios seleccionar asientos, visualizar la disponibilidad y confirmar las reservas.
![Captura Reserva](https://github.com/user-attachments/assets/55776323-cef7-4597-b602-1cb2d61e7e79)

**FRMVenta**: Interfaz de usuario principal que maneja la reserva de butacas, muestra la información de los clientes y genera estadísticas de ventas.
![Captura Venta](https://github.com/user-attachments/assets/0f8c0818-b99c-42e5-84e2-76fd662be520)

## Archivos Importantes
#### Butacas.txt:
Matriz que almacena la disposición y el estado de las butacas en la sala de cine. Cada posición en la matriz representa una butaca y su estado (disponible, reservada, vendida).

#### Clientes.txt:
Archivo que guarda la información de los clientes y sus reservas de butacas.

#### Vendidas.txt:
Archivo que almacena la información de las butacas vendidas, actualizadas a partir de las confirmaciones de venta.

## Requisitos
.NET Framework 4.5 o superior

Visual Studio 2019 o superior

## Contribuciones
Las contribuciones son bienvenidas. Si tienes sugerencias, correcciones o mejoras, por favor envía un pull request.

## Licencia
Este proyecto está licenciado bajo la Licencia MIT. Consulta el archivo LICENSE para obtener más detalles.

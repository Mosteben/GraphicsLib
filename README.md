# Ellipse Rotation Drawing Application

This application implements an algorithm for drawing and rotating ellipses on a graphical interface using C#. It features a user interface with controls for drawing different shapes and manipulating them based on user input.

## Features

- **Ellipse Rotation**: Draws and rotates ellipses based on specified parameters.
- **Shape Drawing**: Supports various drawing algorithms including DDA, Bresenham, and Midpoint Circle.
- **User Interface**: Includes buttons for user interactions, a `DataGridView` for displaying parameters, and a `PictureBox` for rendering shapes.

## Key Components

### Methods

#### `public void ellipserot(int Xc = 0, int Yc = 0, int Rx = 0, int Ry = 0)`

This method handles the drawing and rotation of an ellipse. It takes the following parameters:
- `Xc`: X-coordinate of the center of the ellipse (default is 0).
- `Yc`: Y-coordinate of the center of the ellipse (default is 0).
- `Rx`: Semi-major axis length (default is 0).
- `Ry`: Semi-minor axis length (default is 0).

**Functionality:**
- Initializes the drawing parameters based on user input.
- Uses two regions to calculate the points of the ellipse and update the UI accordingly.
- Updates a `DataGridView` to display the computed values for each point of the ellipse during drawing.

### Event Handlers

- **Button Click Events**: Each button on the interface triggers different functionalities such as starting the drawing process, resetting the canvas, and exiting the application.
  
- **DDA Drawing**: Implements the Digital Differential Analyzer algorithm to draw lines between two points.

- **Bresenham's Line Algorithm**: Efficiently calculates pixel positions to create straight lines.

- **Midpoint Circle Drawing**: Utilizes the midpoint algorithm to create circles based on a specified radius.

## Usage

1. Launch the application.
2. Enter the desired parameters in the text boxes.
3. Click the appropriate button to draw or manipulate the shapes.
4. Observe the updates in the `DataGridView` for computed ellipse parameters.

## Requirements

- .NET Framework (specific version)
- Visual Studio (or compatible IDE)

## License

This project is licensed under the MIT License.

## Acknowledgments

- Thanks to the contributors and documentation sources that helped in implementing the algorithms and UI features.

# Ellipse Rotation Program

This program is designed to rotate an ellipse around a given center point and visualize the process using a DataGridView. It provides functionality to input parameters for the ellipse's center and radii, and displays the calculations and results dynamically.

## Function: `ellipserot(int Xc = 0, int Yc = 0, int Rx = 0, int Ry = 0)`

### Parameters
- **`Xc`**: The x-coordinate of the center of the ellipse (default: 0).
- **`Yc`**: The y-coordinate of the center of the ellipse (default: 0).
- **`Rx`**: The x-axis radius of the ellipse (default: 0).
- **`Ry`**: The y-axis radius of the ellipse (default: 0).

### Description
The `ellipserot` function calculates and plots points on the ellipse's perimeter, updating a DataGridView with the following columns:
- **K**: Step number.
- **Pk**: Parameter for the ellipse.
- **(Xk+1, Yk+1)**: Coordinates of the current point.
- **2r²Yk+1**: Calculation related to the y-axis.
- **2r²Xk+1**: Calculation related to the x-axis.

### Logic
1. **Initialization**: Set up DataGridView columns and determine the center and radii of the ellipse.
2. **Region 1 Calculation**: Use the midpoint algorithm to calculate points in the first region of the ellipse.
3. **Region 2 Calculation**: Calculate points in the second region once the first region is complete.
4. **Plotting**: Points are plotted using the `plotrot` method, and results are added to the DataGridView.

### Usage
- The function can be called with user-defined parameters or defaults.
- The `DDA_clk`, `Drawax_clk`, `button4_Click`, `button5_Click`, `button1_Click`, `button6_Click`, `button7_Click`, and `button8_Click` methods handle user interactions and initiate drawing based on user selections.

### Additional Features
- The program supports dynamic input through text boxes and updates the visual representation in a PictureBox.
- Users can rotate the ellipse based on button clicks, and the transformations are calculated using trigonometric functions for accurate plotting.

## Example
To draw an ellipse with the center at (50, 50) and radii 30 (Rx) and 20 (Ry):
```csharp
ellipserot(50, 50, 30, 20);

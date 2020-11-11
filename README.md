# Documentation

- This is intended to extend the functionality of Unity's Tilemap, by letting you add custom classes (and their respective visuals) to each tile.

# Instructions
1- Create a new custom type (that will be contained within each tile) that extends SmartTileObject. 
You can find our example class in this project called "BuildingTileObject".

2- Create a new custom tilemap that extends SmartTilemap<T>, where T is the custom type you created in step 1.
You can find our example class in this project called "BuildingTilemap".

3- (Optional) Create a monobehaviour class to handle the visuals for your custom tilemap that implements IVisualTilemap<T>, where T is the custom type you created in step 1. 
You can find our example class in this project called "BuildingVisualTilemap".

4- Add the classes created in step 2 and 3 to an object that contains the Unity Tilemap.
If you open the Tilemap scene contained within this project, you can find our example Game Object called "Buildings" (nested under the "Grid" Game Object).

# Mapping instructions

## Mapping/Update

- Make sure the compass is stable before moving around (the compass is represented by the different axis of different colors, it's position will determine the origin of your map). You can use the compass to make sure the SLAM is stable and your mapping sends correct datas
- Walk at an **easy pace** so there are not brutal changes between two data captures otherwise ARFoundation will lose the phone tracking and your data may have a problem.
- If you turn, **turn slowly** otherwise, the relocation at turn positions will have issues.
- The datas that will be used for data generation are the data SENT, if a data is captured but NOT SENT it will not be taken into account during the data generation.
- Depending on your data the map generation can take up to 1 hour.

## Update advice
- If you want to **UPDATE** the map, you need to be at the **SAME POSITION** and **SAME ORIENTATION** before starting the scene.
- You can only update a map if it has already been generated.
- In the update, you can scan places that were not previously mapped in the original mapping.
- You can scan multiple updates before effectively run the update.

## Data plan
If you are a free user your plan is capped to a maximum of 400 data packages but we had very good recognition rates with 170 pictures (original mapping) in our office.

## Data quality
To increase the accuracy of relocation, try to update the map at different times of the day and/or with different lighting conditions.
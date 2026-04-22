## 1. Asset Preparation

- [x] 1.1 Create `UIThemeSO` ScriptableObject to define Blood Red (#FF0000) and Void Black (#000000) colors.
- [ ] 1.2 Create a UI-specific Particle Material for `PixelExplosion` (Unlit/Sprites-Default).
- [ ] 1.3 Create a new `VolumeProfile` for the Menu scene with Film Grain, Bloom, and Vignette.

## 2. Core Scripting

- [x] 2.1 Implement `InfernalMenuCircle.cs` to handle the breathing/scaling animation.
- [x] 2.2 Modify `PixelExplosion.cs` (or create `UIPixelExplosion.cs`) to support UI coordinate space and unscaled time.
- [x] 2.3 Create `InfernalButton.cs` (extending or replacing `ButtonHoverEffect`) to trigger disintegration on click.

## 3. Scene Setup & Layout

- [ ] 3.1 Update the `Menu` scene background to solid black.
- [ ] 3.2 Place the "Central Circle" element and attach `InfernalMenuCircle`.
- [ ] 3.3 Configure the `MenuRoot` Canvas for Screen Space - Camera (to allow particle sorting).
- [ ] 3.4 Apply the new Post-Processing Volume to the scene.

## 4. UI Logic Integration

- [x] 4.1 Update `MenuController.cs` to handle the "Descent" transition delay.
- [x] 4.2 Hook up buttons to trigger the `PixelExplosion` before scene loading.
- [x] 4.3 Ensure text visibility is toggled during disintegration.

## 5. Polish & Verification

- [ ] 5.1 Fine-tune Bloom intensity to ensure text remains readable.
- [ ] 5.2 Verify that particles are properly layered on top of the UI.
- [ ] 5.3 Test the full flow from Menu selection to Game scene transition.

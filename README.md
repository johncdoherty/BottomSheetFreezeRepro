# Syncfusion SfBottomSheet iOS Freeze Bug

## Issue

`SfBottomSheet` freezes when dragging to close on iOS when the grabber handle reaches the tab bar zone.

## Environment

- **.NET MAUI:** 10.0.30
- **Syncfusion.Maui.Toolkit:** 1.0.8
- **iOS:** 18.x, 18.2.6

## Reproduction

1. Open app on iOS device with tab bar
2. Select **Syncfusion** mode → **BROKEN** scenario
3. Open bottom sheet
4. Drag grabber handle downward
5. **Result:** Freeze when grabber reaches tab bar (no crash, complete deadlock)

## Root Cause

**Syncfusion bug:** The grabber's gesture recognizer conflicts with iOS tab bar gestures when spatially overlapping.

**Evidence:** Pure MAUI implementation with `PanGestureRecognizer` works perfectly (no freeze), confirming this is Syncfusion-specific, not a MAUI platform issue

## Workaround

Constrain content height with `ScrollView` + `MaximumHeightRequest`:

```xaml
<bottomSheet:SfBottomSheet.BottomSheetContent>
    <Grid RowDefinitions="Auto,Auto,*">
        <Label Grid.Row="0" Text="Title" />
        
        <ScrollView Grid.Row="1" MaximumHeightRequest="600">
            <Border>
                <!-- Expandable content -->
            </Border>
        </ScrollView>
        
        <CollectionView Grid.Row="2" ItemsSource="{Binding Items}" />
    </Grid>
</bottomSheet:SfBottomSheet.BottomSheetContent>
```

**Critical:** `MaximumHeightRequest` must be on the `ScrollView` itself, not on a child element.

## Demo App

**Toggles:**
- 🔷 **Syncfusion** / 📱 **Pure MAUI** - Compare implementations
- 🔴 **BROKEN** / 🟢 **FIXED** - Test workaround (Syncfusion mode only)

**Test:**
1. Pure MAUI mode - No freeze ✅
2. Syncfusion BROKEN - Reproduces freeze ❌
3. Syncfusion FIXED - Workaround prevents freeze ✅

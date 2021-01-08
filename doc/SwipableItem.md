# SwipableItem

## Summary

The `SwipableItem` implements a design of a item inside a listview that have three state.
The state of this control can be change by either swiping the item to the left (far) or swiping the
item to the right (near).

## Platform support

| Feature                                                      | UWA | Android | iOS |
| ------------------------------------------------------------ |:---:|:-------:|:---:|
| Show near state for an SwipableItem Control                  |  -  |    X    |  X  |
| Show far state for an SwipableItem Control                   |  -  |    X    |  X  |

## Prerequisites

You must add this in  {SharedHeadProject}/styles/controls/SwipableItemControl.xaml

```xmal
<Style x:Key="DefaultSwipableItemStyle"
		   TargetType="u:SwipableItem">
		<Setter Property="Template">
			<Setter.Value>
				<ControlTemplate TargetType="u:SwipableItem">
					<Grid Background="White">
						<ContentPresenter x:Name="NearPresenter"
												Content="{TemplateBinding NearContent}"
												ContentTemplate="{TemplateBinding NearContentTemplate}"
												ContentTemplateSelector="{TemplateBinding NearContentTemplateSelector}"
												HorizontalAlignment="Left"/>
							
							<ContentPresenter x:Name="FarPresenter"
												Content="{TemplateBinding FarContent}"
												ContentTemplate="{TemplateBinding FarContentTemplate}"
												ContentTemplateSelector="{TemplateBinding FarContentTemplateSelector}"
												HorizontalAlignment="Right"/>
							
							<ContentPresenter x:Name="MainPresenter"
												Content="{TemplateBinding Content}"
												ContentTemplate="{TemplateBinding ContentTemplate}"
												ContentTemplateSelector="{TemplateBinding ContentTemplateSelector}"
												HorizontalAlignment="{TemplateBinding HorizontalContentAlignment}"
												VerticalAlignment="{TemplateBinding VerticalContentAlignment}"/>
					</Grid>
				</ControlTemplate>
			</Setter.Value>
		</Setter>
	</Style>
```

## Propreties

- **IsAutoReset** - Determines if we reset the control to NotSwiped when the datacontext changes.
- **FarSnapWidth** - Translation to the left when swiping far.
- **NearSnapWidth** - Translation to the right when swiping near.
- **Content** - Content of the center (intial) composant of the SwipableItem Control.
- **FarContent** -  Content of the far element to be shown when the SwipableItem Control swiping to the left.
- **NearContent** - Content of the near element to be shown when the SwipableItem Control swiping to the right.
- **SwipingState** - Change the state of the SwipableItem Control.

## Usage

For exemple, it can be used in a ListView like this:

The ListView in a page:

Do not forget to set ItemClickEnabled="False"
```xaml
<ListView ItemsSource="{Binding [Items]}"
          ItemTemplate="{StaticResource SwipableContent}"
          IsItemClickEnabled="False" />
```

The SwipableItemControl inside of a DateTemplate of a ListView:
```xaml
<DataTemplate x:Key="SwipableContent">
		<control:SwipableItem IsAutoReset="true"
                             FarSnapWidth="100"
                             NearSnapWidth="100"
                             Content="{Binding}"
                             NearContent="{Binding}"
                             FarContent="{Binding}"
                             NearContentTemplate="{StaticResource SwipableItemNearContent}"
                             ContentTemplate="{StaticResource SwipableItemContent}"
                             FarContentTemplate="{StaticResource SwipableItemFarContent}"
                             Style="{StaticResource DefaultSwipableItemStyle}"/>
</DataTemplate>
```

The SwapableItemControl Template:
(Please note the `Button`s, if you want actions on clicks you must add buttons like this)
```xaml
<DataTemplate x:Key="SwipableItemContent">
	<Button Command={Binding MyCommand}>
	<!-- Content--->
		[...]
	</Button>
</DataTemplate>

<DataTemplate x:Key="SwipableItemNearContent">
	<Button Command={Binding MyNearCommand}>
	<!-- Near Content--->
		[...]
	</Button>
</DataTemplate>

<DataTemplate x:Key="SwipableItemFarContent">
	<Button Command={Binding MyFarCommand}>
	<!-- Far Content--->
		[...]
	</Button>
</DataTemplate>
```

## Known Issue

None.

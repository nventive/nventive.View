# Peeking control

## Summary

The `PeekingControl` is a control that make content to peek from the bottom of the application.

## Platform support

| Feature                               | UWA | Android | iOS |
| --------------------------------------|:---:|:-------:|:---:|
| Peek content from the bottom          |  X  |    X    |  X  |

## Prerequisites

First, you must add this into your PeekingControl style. Otherwise, it won't work for android and iOS.
```xml
<Style x:Key="DefaultPeekingControlStyle"
		   TargetType="u:PeekingControl">
	<Setter Property="Template">
		<Setter.Value>
			<ControlTemplate TargetType="u:PeekingControl">
				<Grid Background="{TemplateBinding Background}">
					<VisualStateManager.VisualStateGroups>
						<VisualStateGroup x:Name="DrawerStates">
							<VisualState x:Name="DrawerOpen">
								<Storyboard Duration="0:0:0.1">
									<DoubleAnimation Storyboard.TargetName="PART_RootTranslation"
														Storyboard.TargetProperty="Y"
														To="0"
														Duration="0:0:0.1" />
								</Storyboard>
							</VisualState>
							<VisualState x:Name="DrawerOpening">
								<Storyboard Duration="0:0:0.1">
									<DoubleAnimation Storyboard.TargetName="PART_RootTranslation"
														Storyboard.TargetProperty="Y"
														To="{Binding HidingSize, RelativeSource={RelativeSource Mode=TemplatedParent}}"
														Duration="0:0:0.1" />
								</Storyboard>
							</VisualState>
							<VisualState x:Name="DrawerClosing">
								<Storyboard Duration="0:0:0.1">
									<DoubleAnimation Storyboard.TargetName="PART_RootTranslation"
														Storyboard.TargetProperty="Y"
														To="0"
														Duration="0:0:0.1" />
								</Storyboard>
							</VisualState>
							<VisualState x:Name="DrawerClosed">
								<Storyboard Duration="0:0:0.1">
									<DoubleAnimation Storyboard.TargetName="PART_RootTranslation"
														Storyboard.TargetProperty="Y"
														To="{Binding HidingSize, RelativeSource={RelativeSource Mode=TemplatedParent}}"
														Duration="0:0:0.1" />
								</Storyboard>
							</VisualState>
						</VisualStateGroup>
					</VisualStateManager.VisualStateGroups>

					<Grid.RowDefinitions>
						<RowDefinition Height="Auto" />
						<RowDefinition Height="*" />
					</Grid.RowDefinitions>

					<Grid.RenderTransform>
						<TranslateTransform x:Name="PART_RootTranslation"
											Y="0" />
					</Grid.RenderTransform>

					<Grid x:Name="PART_Chevron"
							Height="{TemplateBinding PeekingSize}"
							Background="#40000000">
						<TextBlock Text="{TemplateBinding Header}"
									HorizontalAlignment="Center"
									VerticalAlignment="Center"
									Margin="5" />
					</Grid>

					<ContentPresenter Grid.Row="1"
										Content="{TemplateBinding Content}"
										ContentTemplate="{TemplateBinding ContentTemplate}"
										ContentTemplateSelector="{TemplateBinding ContentTemplateSelector}"
										HorizontalAlignment="Stretch"
										VerticalAlignment="Stretch" />
				</Grid>
			</ControlTemplate>
		</Setter.Value>
	</Setter>
</Style>
```

Second. you need to import this in your page.
```xml
xmlns:u="using:Umbrella.View.Controls"
```

## Properties

- **Header** - The text that will be shown in the header of the peeking control.

## Usage

```xml
<u:PeekingControl Header="This is a PeekingControl"
                  Style="{StaticResource DefaultPeekingControlStyle}">
    <!-- Content of the Peeking control -->
    [...]
</u:PeekingControl>
````

## Known issue

None.

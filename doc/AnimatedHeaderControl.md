# AnimatedHeaderControl

## Summary

The `AnimatedHeaderControl` implements the design idiom of a masthead (main header) shown at the top of a block of scrollable content, with 
visual effects as the masthead is scrolled out of view. 

Currently the control exposes a `RelativeOffset` dependency property whose value is always between 0 and 1, where 0 indicates 'scroll is at 
the top, masthead is fully visible' and 1 indicates 'masthead is fully scrolled out of view'. 


## Platform support

| Feature                                                      | UWA | Android | iOS |
| ------------------------------------------------------------ |:---:|:-------:|:---:|
| RelativeOffset property                                      |  X  |    X    |  X  |
| IsMasthead attached property                                 |  X  |    X    |  X  |
| ScrollViewer in content                                      |  X  |    X    |  X  |
| ListView/GridView in content                                 |  X  |    X    |  X  |
| Binding to RelativeOffset inside template using ElementName  |  X  |         |     |

## Usage

 * Use scrollable content (a `ScrollViewer` or a `ListView`/`GridView`) as the content of `AnimatedHeaderControl`.
 * Mark an element inside the content as the masthead to calculate `RelativeOffset' from, or manually set the `MastheadEnd` property. 

````	<Grid Width="300"
		  Height="500"
		  Background="White">
		<u:AnimatedHeaderControl x:Name="AnimatedHeaderControl"
								 MastheadTopMargin="{Binding Height, ElementName=FixedHeader}"
								 RelativeOffset="{Binding [RelativeOffset], Mode=TwoWay}"
								 AbsoluteOffset="{Binding [AbsoluteOffset], Mode=TwoWay}">
			<ScrollViewer>
				<StackPanel Margin="0,50,0,0">
					<Border Height="200"
							u:AnimatedHeaderControl.IsMasthead="True">
						<Border.Background>
							<LinearGradientBrush StartPoint="0,.5"
												 EndPoint="1,.5">
								<GradientStop Color="Pink"
											  Offset="{Binding [RelativeOffset]}" />
								<GradientStop Color="Red"
											  Offset="1" />
							</LinearGradientBrush>
						</Border.Background>
					</Border>
					<TextBlock Text="RelativeOffset:" />
					<TextBlock Text="{Binding [RelativeOffset]}" />
					<TextBlock Text="AbsoluteOffset:" />
					<TextBlock Text="{Binding [AbsoluteOffset]}" />
					<Rectangle Height="5"
							   Fill="Gray" />
					<TextBlock TextWrapping="Wrap"
							   Text="Long text string" />
				</StackPanel>
			</ScrollViewer>
		</u:AnimatedHeaderControl>
		<Grid x:Name="FixedHeader"
			  Height="50"
			  VerticalAlignment="Top">
			<Border Background="Blue"
					Opacity="{Binding RelativeOffset, ElementName=AnimatedHeaderControl}">
			</Border>
			<TextBlock Text="Fixed header" />
		</Grid>
	</Grid>
````

## Known issues

Known Uno limitation: it's not possible to bind inside a template using ElementName to a named element outside the template. The workaround 
to bind to RelativeOffset inside the AnimatedHeaderControl's content is to two-way bind it to the view model.
# StickyGroupHeaderBehavior

## Summary

The `StickyGroupHeaderBehavior` allows modifying the appearance/behavior of a ListViewBase group header based on whether that group is 
currently 'sticking' to the top of the scrollable viewport.


## Platform support

| Feature                                                    | UWA | Android | iOS |
| ---------------------------------------------------------- |:---:|:-------:|:---:|
| IsSticking attached property inside header template        |  X  |    X    |  X  |
| CurrentlyStickingGroup attached property on ListViewBase   |  X  |    X    |  X  |

## Usage

`StickyGroupHeaderBehavior.IsEnabled="True"` must be set both on any elements wishing to bind to `StickyGroupHeaderBehavior.IsSticking`, 
_and_ on the parent ListView/GridView.

Example usage:

````
	<ListView Width="300"
				Height="400"
				ItemsSource="{Binding [LotsOfStringsGrouped]}"
				ue:StickyGroupHeaderBehavior.IsEnabled="True"
				Background="White">
		<ListView.GroupStyle>
			<GroupStyle>
				<GroupStyle.HeaderTemplate>
					<DataTemplate>
						<Border BorderBrush="Blue"
								BorderThickness="2"
								Width="200">
							<StackPanel>
								<TextBlock Text="{Binding Key}"
											Foreground="Blue" />
								<StackPanel Orientation="Horizontal">
									<TextBlock Text="Is sticky = " />
									<TextBlock ue:StickyGroupHeaderBehavior.IsEnabled="True"
												x:Name="ShowSticky"
												Text="{Binding Path=(ue:StickyGroupHeaderBehavior.IsSticking), ElementName=ShowSticky}" />
								</StackPanel>
							</StackPanel>
						</Border>
					</DataTemplate>
				</GroupStyle.HeaderTemplate>
			</GroupStyle>
		</ListView.GroupStyle>
	</ListView>
````

## Known issues

Uno currently doesn't support RelativeMode=Self in bindings, use ElementName instead.

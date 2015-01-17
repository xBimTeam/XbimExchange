param($installPath, $toolsPath, $package, $project)

$propertyName = "CopyToOutputDirectory"

$item32 = $project.ProjectItems.Item("COBieAttributes.config")
if ($item32  -ne $null) 
{ 
	$property = $item32.Properties.Item($propertyName)
	if ($property -ne $null) 
	{ 
		$property.Value = 2
	}
}


require 'albacore'

task :default => :build

desc "Build solution"
msbuild :build do |msb|
  msb.properties :configuration => :Debug
  msb.targets :Clean, :Build
  msb.solution = "Scaffold.sln"
end

desc "Clean solution"
msbuild :clean do |msb|
  msb.properties :configuration => :Debug
  msb.targets :Clean
  msb.solution = "Scaffold.sln"
end

desc "Tests"
xunit :test => :build do |xunit|
	xunit.command = "Tools/XUnit/xunit.console.clr4.x86.exe"
	xunit.assembly = "Scaffold.Test/bin/Debug/Scaffold.Test.dll"
end

desc "Package"
task :package => :build do
  rm_rf "Scaffold.zip"
  zipPackage()
  puts "Scaffold.zip is ready"
end

def zipPackage 
	ZipFile.open("Scaffold.zip", 'w')  do |zipfile|
		Dir["Scaffold/bin/Debug/**/**"].each do |file_path|
			file_name = file_path.sub("Scaffold/bin/Debug/" , "")
			puts "Adding #{file_name}"
			zipfile.add(file_name, file_path)
		end
		Dir["Templates/**/**"].each do |file_path|
			file_name = file_path
			puts "Adding #{file_name}"
			zipfile.add(file_name, file_path)
		end
    end
end
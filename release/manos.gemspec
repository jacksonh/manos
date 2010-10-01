version = '0.0.2'

Gem::Specification.new do |spec|
  spec.platform    = Gem::Platform::CURRENT
  spec.name        = 'manos'
  spec.version     = version
  spec.files 	   = Dir['build/**/*'] + Dir['docs/**/*']

  spec.summary     = 'Manos de Mono is a web framework for use with Mono.'
  spec.description = <<-EOF
	Manos is an easy to use, easy to test, high performance web application framework 
	that stays out of your way and makes your life ridiculously simple.
  EOF

  spec.authors           = 'Jackson Harper'
  spec.email             = 'manos-de-mono@googlegroups.com'
  spec.homepage          = 'http://manos-de-mono.com'
  spec.rubyforge_project = 'manos'
end  

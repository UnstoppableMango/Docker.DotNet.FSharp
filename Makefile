_ := $(shell mkdir -p .make)

.PHONY: build test lint clean

build:
	dotnet build

restore: .make/restore_tools
	dotnet restore

test:
	dotnet test

lint: .make/restore_tools
	dotnet fantomas .

clean:
	@find . -type d \
		\( -name 'bin' -o -name 'obj' \) \
		-exec rm -rf '{}' + \
		-ls

.make/restore_tools: .config/dotnet-tools.json
	dotnet tool restore
	@touch $@

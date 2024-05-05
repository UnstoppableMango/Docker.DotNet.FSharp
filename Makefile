WORKING_DIR = $(shell pwd)
FANTOMAS    = dotnet fantomas
_ := $(shell mkdir -p .make)

VERSION := $(shell dotnet minver --tag-prefix v --verbosity warn)
export MINVERVERSIONOVERRIDE = ${VERSION}

.PHONY: all build test lint clean pack version
all: build check test

build: .make/build
restore: .make/restore_tools .make/restore

test: .make/build
	dotnet test --no-build

check: .make/lint_check
lint: .make/lint

clean:
	rm -rf .make out
	@find . -type d \
		\( -name 'bin' -o -name 'obj' \) \
		-exec rm -rf '{}' + \
		-ls

pack: out/UnMango.Docker.DotNet.FSharp.$(VERSION).nupkg

version:
	@echo '${VERSION}'

.PHONY: devcontainer
devcontainer: CLI_VER := $(shell cat .versions/devcontainers_cli)
devcontainer: CLI := npx @devcontainers/cli@$(CLI_VER)
devcontainer: .make/ensure_npx .versions/devcontainers_cli
	${CLI} build --workspace-folder '${WORKING_DIR}'

PROJECT_FILE := src/Docker.DotNet.FSharp/FSharp.fsproj
SRC := $(shell find src -maxdepth 2 -type f -name '*.fs')

out/UnMango.Docker.DotNet.FSharp.$(VERSION).nupkg: $(SRC) $(PROJECT_FILE) README.md
	dotnet pack --output out

.make/restore: $(PROJECT_FILE)
	dotnet restore
	@touch $@

.make/build: $(SRC) $(PROJECT_FILE) .make/restore
	dotnet build --no-restore
	@touch $@

.make/lint: $(SRC) .make/restore_tools
	${FANTOMAS} ${WORKING_DIR}
	@touch $@

.make/lint_check: $(SRC) .make/restore_tools
	${FANTOMAS} ${WORKING_DIR} --check
	@touch $@

.make/restore_tools: .config/dotnet-tools.json
	dotnet tool restore
	@touch $@

.make/ensure_npx:
	@bin="$$(which npx)" && echo "$$bin" > $@ || echo 'Install Node.js first' && exit 1

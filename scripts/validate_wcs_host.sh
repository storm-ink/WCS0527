#!/usr/bin/env bash

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
WCS_ROOT="$ROOT_DIR/艾芬达6号库WCS"
HOST_PROJECT="$WCS_ROOT/v1.2/WcsConsoleApplication/WcsConsoleApplication.csproj"
HOST_EXE="$WCS_ROOT/v1.2/WcsConsoleApplication/bin/Debug/WcsConsoleApplication.exe"
DEFAULT_CONFIG="$WCS_ROOT/ZHQXC/Wcs.App.exe.config"
DEFAULT_BASE_DIR="$WCS_ROOT/ZHQXC"

usage() {
  cat <<'EOF'
Usage:
  validate_wcs_host.sh build
  validate_wcs_host.sh help
  validate_wcs_host.sh validate [config_path] [base_dir]

Commands:
  build      Rebuild the WcsConsoleApplication host.
  help       Print the host help output.
  validate   Run host validation against an external config and base directory.

Defaults for validate:
  config_path = 艾芬达6号库WCS/ZHQXC/Wcs.App.exe.config
  base_dir    = 艾芬达6号库WCS/ZHQXC
EOF
}

ensure_dependencies() {
  command -v xbuild >/dev/null 2>&1 || {
    echo "xbuild is required but not found." >&2
    exit 1
  }

  command -v mono >/dev/null 2>&1 || {
    echo "mono is required but not found." >&2
    exit 1
  }
}

build_host() {
  ensure_dependencies
  xbuild "$HOST_PROJECT" /t:Rebuild /p:Configuration=Debug
}

show_help() {
  ensure_dependencies
  mono "$HOST_EXE" --help
}

validate_host() {
  ensure_dependencies
  local config_path="${1:-$DEFAULT_CONFIG}"
  local base_dir="${2:-$DEFAULT_BASE_DIR}"
  mono "$HOST_EXE" --config "$config_path" --base-directory "$base_dir" --validate-config
}

main() {
  local command="${1:-}"

  case "$command" in
    build)
      build_host
      ;;
    help)
      show_help
      ;;
    validate)
      shift
      validate_host "${1:-}" "${2:-}"
      ;;
    ""|-h|--help)
      usage
      ;;
    *)
      echo "Unknown command: $command" >&2
      usage >&2
      exit 1
      ;;
  esac
}

main "$@"

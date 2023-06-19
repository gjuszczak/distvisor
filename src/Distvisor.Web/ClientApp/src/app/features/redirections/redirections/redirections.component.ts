import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store, Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { LazyLoadEvent, MenuItem } from 'primeng/api';

import { Redirection, RedirectionModal, RedirectionModalKind, RedirectionsList, RedirectionsState, emptyRedirection } from '../store/redirections.state';
import { CreateRedirection, DeleteRedirection, EditRedirection, HideRedirectionModal, LoadRedirections, ShowRedirectionModal } from '../store/redirections.actions';

@Component({
  selector: 'app-redirections',
  templateUrl: './redirections.component.html',
})
export class RedirectionsComponent implements OnInit {

  @Select(RedirectionsState.getRedirections)
  public readonly redirections$!: Observable<RedirectionsList>;

  @Select(RedirectionsState.getModal)
  public readonly modal$!: Observable<RedirectionModal>;

  public readonly RedirectionModalKind = RedirectionModalKind;

  public redirectionMenuItems: MenuItem[] = [];
  public selectedRedirection?: Redirection;
  private firstLazyLoad: boolean = true;

  constructor(
    private readonly store: Store,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.redirectionMenuItems = [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => {
          if (this.selectedRedirection) {
            this.store.dispatch(new ShowRedirectionModal(RedirectionModalKind.Edit, this.selectedRedirection));
          }
        }
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {
          if (this.selectedRedirection) {
            this.store.dispatch(new ShowRedirectionModal(RedirectionModalKind.ConfirmDelete, this.selectedRedirection));
          }
        }
      }
    ];

    this.route.queryParams.subscribe(({ first, rows }) => {
      this.reloadRedirections(first, rows);
    });
  }

  toggleRedirectionMenu(menu: any, event: any, redirection: Redirection) {
    this.selectedRedirection = redirection;
    menu.toggle(event);
  }

  showCreateRedirectionModal() {
    this.store.dispatch(new ShowRedirectionModal(RedirectionModalKind.Create, emptyRedirection));
  }

  lazyLoadRedirections({ first, rows }: LazyLoadEvent) {
    if (this.firstLazyLoad) {
      this.firstLazyLoad = false;
      return;
    }

    this.router.navigate([], { queryParams: { first, rows } });
  }

  reloadRedirections(first?: number, rows?: number) {
    this.store.dispatch(new LoadRedirections(first, rows));
  }

  hideRedirectionModal() {
    this.store.dispatch(new HideRedirectionModal());
  }

  createRedirection(value: Redirection) {
    this.store.dispatch(new CreateRedirection(value.name, value.url));
  }

  editRedirection(value: Redirection) {
    this.store.dispatch(new EditRedirection(value.id, value.name, value.url));
  }

  deleteRedirection(value: Redirection) {
    this.store.dispatch(new DeleteRedirection(value.id));
  }
}
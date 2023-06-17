export class Paginated<T> {
  entries: T[];
  pagesAmount: number;

  constructor(entries: T[], pagesAmount: number) {
    this.entries = entries;
    this.pagesAmount = pagesAmount;
  }
}
